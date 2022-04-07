namespace AspNetCore.Aws.S3.Simple.Models;

public readonly struct FileUploadResult
{
    public FileUploadResult(
        string uniqueStorageName,
        string errorReason)
    {
        UniqueStorageName = uniqueStorageName;
        ErrorReason = errorReason;
    }

    public string UniqueStorageName { get; }

    public bool Result => !string.IsNullOrEmpty(UniqueStorageName);

    public string ErrorReason { get; }

    public static FileUploadResult Success(string uniqueStorageName) => new FileUploadResult(uniqueStorageName, null);

    public static FileUploadResult Failure(string errorReason) => new FileUploadResult(null, errorReason);
}