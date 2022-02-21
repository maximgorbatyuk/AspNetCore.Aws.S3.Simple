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

        var result = await _reimbursementFileStorage.UploadFileAsync(new UploadFileRequest(file.File));
        return Ok(result.UniqueStorageName);
    }

    [HttpPost("download")]
    public async Task<IActionResult> DownloadAsync([FromBody] FileRequest request)
    {
        var file = await _reimbursementFileStorage.DownloadFileAsync(request.Filename);
        return File(file.Content, file.ContentType, file.OriginalFileName);
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteAsync([FromBody] FileRequest request)
    {
        return Ok(await _reimbursementFileStorage.DeleteFileAsync(request.Filename));
    }
}

public record FormUploadRequest
{
    public IFormFile? File { get; init; }
}

public class FileRequest
{
    public string? Filename { get; init; }
}