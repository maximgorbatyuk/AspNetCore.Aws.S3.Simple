using System;

namespace S3.Integration.Models;

public struct FileUploadResult
{
    public FileUploadResult(bool success, string uniqueStorageName)
    {
        Success = success;
        UniqueStorageName = uniqueStorageName;
    }

    public bool Success { get; }

    public string UniqueStorageName { get; }
}