using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;
using S3.Integration.Contracts;
using S3.Integration.Exceptions;
using S3.Integration.Models;
using S3.Integration.Settings;
using S3.Integration.Utils;

namespace S3.Integration.AmazonServices;

public abstract class AmazonS3StorageBase : IFileStorageBase
{
    private const string OriginalFileNameMetaKey = "x-amz-meta-title";
    private const string ContentTypeMetaKey = "x-amz-meta-content-type";
    private const int FilePartitionSize = 6_291_456;

    private readonly S3StorageSettings _settings;
    private readonly IS3FileValidator _fileValidator;
    private readonly string _folderPrefix;

    protected AmazonS3StorageBase(
        S3StorageSettings configuration,
        IS3FileValidator fileValidator,
        string folderPrefix)
    {
        _fileValidator = fileValidator;
        _folderPrefix = folderPrefix;
        _settings = configuration;
    }

    public async Task<IOptional<string>> UploadFileAsync(IUploadFileRequest file)
    {
        using var client = _settings.CreateClient();
        if (!await CreateBucketIfNotExistAsync(client))
        {
            throw new InvalidOperationException("Could not create bucket");
        }

        var fileTransferUtility = new TransferUtility(client);

        if (!_fileValidator.IsValid(file))
        {
            return new FileUploadResult(string.Empty, null);
        }

        var randomFileName = new RandomFileName(file.FileName);
        var uniqueStorageKey = WithFolderPrefixIfExist(randomFileName);

        // Create the image object to be uploaded in memory
        await using var fileStream = file.OpenReadStream();
        var transferUtilityRequest = new TransferUtilityUploadRequest
        {
            InputStream = fileStream,
            Key = uniqueStorageKey,
            BucketName = _settings.BucketName,
            CannedACL = S3CannedACL.PublicRead, // Ensure the file is read-only to allow users view their pictures
            PartSize = FilePartitionSize
        };

        transferUtilityRequest.Metadata.Add(OriginalFileNameMetaKey, randomFileName.OriginalFileName);
        transferUtilityRequest.Metadata.Add(ContentTypeMetaKey, file.ContentType);

        try
        {
            await fileTransferUtility.UploadAsync(transferUtilityRequest);
            return new FileUploadResult(uniqueStorageKey, null);
        }
        catch (Exception e)
        {
            return new FileUploadResult(string.Empty, e);
        }
    }

    public async Task<IOptional<S3File>> DownloadFileAsync(string uniqueStorageName)
    {
        using var client = _settings.CreateClient();
        if (!await DoesBucketExistAsync(client))
        {
            return new Optional<S3File>(
                null, new InvalidOperationException("Bucket does not exist"));
        }

        var request = new FileDownloadRequest(uniqueStorageName, _settings.BucketName);

        try
        {
            using GetObjectResponse response = await client.GetObjectAsync(request);
            await using var responseStream = response.ResponseStream;
            await using var memory = new MemoryStream();

            var originalFileName = response.Metadata[OriginalFileNameMetaKey];
            var contentType = response.Metadata[ContentTypeMetaKey];
            await responseStream.CopyToAsync(memory);
            var responseBody = memory.ToArray();

            return new Optional<S3File>(
                new S3File(
                    originalFileName,
                    uniqueStorageName,
                    contentType,
                    responseBody,
                    response.LastModified));
        }
        catch (Exception ex)
        {
            return new Optional<S3File>(null, ex);
        }
    }

    public async Task<IOptional<bool>> DeleteFileAsync(string uniqueStorageName)
    {
        using var client = _settings.CreateClient();
        if (!await DoesBucketExistAsync(client))
        {
            return new Optional<bool>(
                false, new InvalidOperationException("Bucket does not exist"));
        }

        var request = new FileDeleteRequest(uniqueStorageName, _settings.BucketName);

        try
        {
            var response = await client.DeleteObjectAsync(request);
            var deleteMarker = response.DeleteMarker;

            return new Optional<bool>(!string.IsNullOrEmpty(deleteMarker), null);
        }
        catch (Exception ex)
        {
            return new Optional<bool>(false, ex);
        }
    }

    private async Task<bool> CreateBucketIfNotExistAsync(
        IAmazonS3 client)
    {
        try
        {
            if (await DoesBucketExistAsync(client))
            {
                return true;
            }

            var putBucketRequest = new PutBucketRequest
            {
                BucketName = _settings.BucketName,
                UseClientRegion = true
            };

            var response = await client.PutBucketAsync(putBucketRequest);
            return response.HttpStatusCode == HttpStatusCode.OK;
        }
        catch (AmazonS3Exception e)
        {
            throw new S3StorageException($"Could not create bucket {_settings}", e);
        }
    }

    private Task<bool> DoesBucketExistAsync(
        IAmazonS3 client) =>
        AmazonS3Util.DoesS3BucketExistV2Async(client, _settings.BucketName);

    private string WithFolderPrefixIfExist(
        RandomFileName fileName)
    {
        if (string.IsNullOrEmpty(_folderPrefix))
        {
            return fileName.ToString();
        }

        return _folderPrefix + "/" + fileName;
    }
}