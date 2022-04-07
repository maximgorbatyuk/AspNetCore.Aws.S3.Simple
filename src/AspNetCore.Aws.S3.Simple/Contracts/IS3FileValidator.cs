using AspNetCore.Aws.S3.Simple.Models;

namespace AspNetCore.Aws.S3.Simple.Contracts;

public interface IS3FileValidator
{
    FileValidationResult Validate(IUploadFileRequest uploadFileRequest);
}