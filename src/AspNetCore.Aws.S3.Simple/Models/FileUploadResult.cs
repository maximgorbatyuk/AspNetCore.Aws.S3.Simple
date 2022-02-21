namespace AspNetCore.Aws.S3.Simple.Models;

public readonly struct FileUploadResult
{
    public FileUploadResult(
        string uniqueStorageName)
    {
        UniqueStorageName = uniqueStorageName;
    }

    public string UniqueStorageName { get; }

    public bool Success => !string.IsNullOrEmpty(UniqueStorageName);
}