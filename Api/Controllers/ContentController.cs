using Microsoft.AspNetCore.Mvc;
using Api.Interfaces;

namespace Api.Controllers;

[ApiController]
[Route("api/content")]
public class ContentController : ControllerBase
{
    private readonly IContentService _contentService;
    private readonly IUserResolutionService _userResolver;

    public ContentController(
        IContentService contentService,
        IUserResolutionService userResolver)
    {
        _contentService = contentService;
        _userResolver = userResolver;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var user = await _userResolver.ResolveAsync(HttpContext);

        var items = await _contentService.GetAllAsync(user.Id);
        return Ok(items);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _userResolver.ResolveAsync(HttpContext);

        var item = await _contentService.GetByIdAsync(id, user.Id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpGet("{id:guid}/audios")]
    public async Task<IActionResult> GetAudios(Guid id)
    {
        var user = await _userResolver.ResolveAsync(HttpContext);

        var audios = await _contentService.GetAudiosAsync(id, user.Id);
        return Ok(audios);
    }
}