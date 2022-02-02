using S3.Integration.Models;

namespace S3.Integration.Contracts;

public interface IS3FileValidator
{
    bool IsValid(IUploadFileRequest uploadFileRequest);
}