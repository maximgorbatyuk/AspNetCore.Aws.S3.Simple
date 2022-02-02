using System.Collections.Generic;
using System.IO;
using System.Linq;
using S3.Integration.Contracts;
using S3.Integration.Models;
using S3.Integration.Settings;

namespace S3.Integration.AmazonServices;

public class S3FileValidator : IS3FileValidator
{
    private readonly S3StorageSettings _settings;

    public S3FileValidator(S3StorageSettings settings)
    {
        _settings = settings;
    }

    public bool IsValid(IUploadFileRequest uploadFileRequest)
    {
        // Check file length
        if (uploadFileRequest.FileSize < 0)
        {
            return false;
        }

        var ext = Path.GetExtension(uploadFileRequest.FileName)?.ToLowerInvariant();
        if (string.IsNullOrEmpty(ext) || !AllowedExtensions().Contains(ext))
        {
            return false;
        }

        // Check if file size is greater than permitted limit
        return uploadFileRequest.FileSize <= FileSize();
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