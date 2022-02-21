using System.Threading.Tasks;
using S3.Integration.Models;

namespace S3.Integration.Contracts;

public interface IFileStorageBase
{
    Task<IOptional<string>> UploadFileAsync(IUploadFileRequest file);

    Task<IOptional<S3File>> DownloadFileAsync(string uniqueStorageName);

    Task<IOptional<bool>> DeleteFileAsync(string uniqueStorageName);
}