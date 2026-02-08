using Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Api.Contracts;

[ApiController]
[Route("admin/content")]
[Authorize(Policy = "AdminOnly")]
public class AdminContentController : ControllerBase
{
    private readonly IContentWriteService _writes;

    public AdminContentController(IContentWriteService writes)
    {
        _writes = writes;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateContentRequest request)
    {
        var id = await _writes.CreateAsync(request);
        return Ok(new { id });
    }

    [HttpPost("{id}/audio")]
    public async Task<IActionResult> AddAudioVariant(
        Guid id,
        CreateAudioVariantRequest request)
    {
        await _writes.AddAudioVariantAsync(id, request);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _writes.DeleteAsync(id);
        return NoContent();
    }
}
