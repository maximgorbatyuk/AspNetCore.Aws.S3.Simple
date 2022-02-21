using System.Threading.Tasks;
using AspNetCore.Aws.S3.Simple.Models;

namespace AspNetCore.Aws.S3.Simple.Contracts;

public interface IFileStorageBase
{
    Task<FileUploadResult> UploadFileAsync(IUploadFileRequest file);

    Task<S3File> DownloadFileAsync(string uniqueStorageName);

    Task<bool> DeleteFileAsync(string uniqueStorageName);
}