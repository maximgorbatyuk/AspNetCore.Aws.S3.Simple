namespace AspNetCore.Aws.S3.Simple.Models;

public enum FileValidationResult
{
    Unknown,
    Valid,
    FilesizeExceeded,
    FilesizeInvalid,
    FileExtensionEmpty,
    FileExtensionNotAllowed,
}