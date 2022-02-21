using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCore.Aws.S3.Simple.Models;

public interface IUploadFileRequest
{
    /// <summary>
    /// Gets the file length in bytes.
    /// </summary>
    long FileSize { get; }

    /// <summary>
    /// Gets the file name from the Content-Disposition header.
    /// </summary>
    string FileName { get; }

    string ContentType { get; }

    /// <summary>
    /// Opens the request stream for reading the uploaded file.
    /// </summary>
    /// <returns>Stream.</returns>
    Stream OpenReadStream();

    /// <summary>
    /// Copies the contents of the uploaded file to the <paramref name="target"/> stream.
    /// </summary>
    /// <param name="target">The stream to copy the file contents to.</param>
    void CopyTo(Stream target);

    /// <summary>
    /// Asynchronously copies the contents of the uploaded file to the <paramref name="target"/> stream.
    /// </summary>
    /// <param name="target">The stream to copy the file contents to.</param>
    /// <param name="cancellationToken">cancellation Token.</param>
    /// <returns>Task.</returns>
    Task CopyToAsync(Stream target, CancellationToken cancellationToken = default);
}