using Microsoft.AspNetCore.Mvc;
using Sample.Api.Services;

namespace Sample.Api.Controllers;

[ApiController]
[Route("files")]
public class FilesDebugController : ControllerBase
{
    private readonly IReimbursementFileStorage _reimbursementFileStorage;

    public FilesDebugController(IReimbursementFileStorage reimbursementFileStorage)
    {
        _reimbursementFileStorage = reimbursementFileStorage;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadAsync([FromForm] FormUploadRequest? file)
    {
        if (file?.File == null)
        {
            return BadRequest();
        }

        return Ok(await _reimbursementFileStorage.UploadFileAsync(new UploadFileRequest(file.File)));
    }

    [HttpPost("download")]
    public async Task<FileContentResult> DownloadAsync([FromBody] FileDownloadRequest request)
    {
        var file = await _reimbursementFileStorage.DownloadFileAsync(request.Filename);
        return File(file.Content, file.ContentType, file.OriginalFileName);
    }
}

public record FormUploadRequest
{
    public IFormFile? File { get; init; }
}

public class FileDownloadRequest
{
    public string? Filename { get; init; }
}