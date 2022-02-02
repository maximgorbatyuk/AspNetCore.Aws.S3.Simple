using S3.Integration.Models;

namespace Sample.Api.Services;

public class UploadFileRequest : IUploadFileRequest
{
    private readonly IFormFile _formFile;

    public UploadFileRequest(IFormFile formFile)
    {
        _formFile = formFile;
    }

    public long FileSize => _formFile.Length;

    public string FileName => _formFile.FileName;

    public string ContentType => _formFile.ContentType;

    public Stream OpenReadStream() => _formFile.OpenReadStream();

    public void CopyTo(Stream target) => _formFile.CopyTo(target);

    public Task CopyToAsync(Stream target, CancellationToken cancellationToken = default) =>
        _formFile.CopyToAsync(target, cancellationToken);
}