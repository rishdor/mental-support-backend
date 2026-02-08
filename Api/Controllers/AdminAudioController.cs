using Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Api.Contracts;

[ApiController]
[Route("admin/audio")]
[Authorize(Policy = "AdminOnly")]
public class AdminAudioController : ControllerBase
{
    private readonly IAudioStorageService _storage;

    public AdminAudioController(IAudioStorageService storage)
    {
        _storage = storage;
    }

    [HttpPost]
    [RequestSizeLimit(50_000_000)]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Upload([FromForm] AudioUploadRequest request)
    {
        if (request.File == null || request.File.Length == 0)
            return BadRequest("No file uploaded.");

        var result = await _storage.UploadAsync(request.File);
        return Ok(result);
    }
}