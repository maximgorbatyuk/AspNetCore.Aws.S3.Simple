using System.Threading.Tasks;
using S3.Integration.Models;

namespace S3.Integration.Contracts;

public interface IFileStorageBase
{
    Task<FileUploadResult> UploadFileAsync(IUploadFileRequest file);

    Task<S3File> DownloadFileAsync(string uniqueStorageName);
}