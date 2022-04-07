using System.Collections.Generic;
using System.IO;
using System.Linq;
using AspNetCore.Aws.S3.Simple.Contracts;
using AspNetCore.Aws.S3.Simple.Models;
using AspNetCore.Aws.S3.Simple.Settings;

namespace AspNetCore.Aws.S3.Simple.AmazonServices;

public class S3FileValidator : IS3FileValidator
{
    private readonly S3StorageSettings _settings;

    public S3FileValidator(S3StorageSettings settings)
    {
        _settings = settings;
    }

    public FileValidationResult Validate(IUploadFileRequest uploadFileRequest)
    {
        // Check file length
        if (uploadFileRequest.FileSize < 0)
        {
            return FileValidationResult.FilesizeInvalid;
        }

        var ext = Path.GetExtension(uploadFileRequest.FileName)?.ToLowerInvariant();
        if (string.IsNullOrEmpty(ext))
        {
            return FileValidationResult.FileExtensionEmpty;
        }

        if (!AllowedExtensions().Contains(ext))
        {
            return FileValidationResult.FileExtensionNotAllowed;
        }

        // Check if file size is greater than permitted limit
        return uploadFileRequest.FileSize <= FileSize()
            ? FileValidationResult.Valid
            : FileValidationResult.FilesizeExceeded;
    }

    private ICollection<string> AllowedExtensions()
    {
        return _settings.AllowedFileExtensions
            .Split(",")
            .Select(x => "." + x)
            .ToList();
    }

    private int FileSize()
    {
        const int rate = 1024;
        return _settings.AllowedFileSizeInMb * rate * rate;
    }
}