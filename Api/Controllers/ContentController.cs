using Microsoft.AspNetCore.Mvc;
using Api.Services;

namespace Api.Controllers;

[ApiController]
[Route("api/content")]
public class ContentController : ControllerBase
{
    private readonly ContentService _contentService;
    private readonly UserResolutionService _userResolver;

    public ContentController(
        ContentService contentService,
        UserResolutionService userResolver)
    {
        _contentService = contentService;
        _userResolver = userResolver;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _userResolver.ResolveAsync(HttpContext);
        var user = result.user;

        var items = await _contentService.GetAllAsync(user.Id);
        return Ok(items);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _userResolver.ResolveAsync(HttpContext);
        var user = result.user;

        var item = await _contentService.GetByIdAsync(id, user.Id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpGet("{id:guid}/audios")]
    public async Task<IActionResult> GetAudios(Guid id)
    {
        var result = await _userResolver.ResolveAsync(HttpContext);
        var user = result.user;

        var audios = await _contentService.GetAudiosAsync(id, user.Id);
        return Ok(audios);
    }
}