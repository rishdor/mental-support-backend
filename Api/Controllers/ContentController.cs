using Microsoft.AspNetCore.Mvc;
using Api.Services;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContentController : ControllerBase
{
    private readonly ContentService _contentService;
    public ContentController(ContentService contentService)
    {
        _contentService = contentService;
    }    

    [Route("all")]
    [HttpGet]
    public IActionResult GetAllContentItems()
    {
        try
        {
            var contentItems = _contentService.GetAllContentItems();
            return Ok(contentItems);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving content items.", detail = ex.Message });
            
        }
    }

    [Route("{id}")]
    [HttpGet]
    public IActionResult GetContentItemById([FromRoute] Guid id)
    {
        try
        {
            var contentItem = _contentService.GetContentItemById(id);
            return Ok(contentItem);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "Content item not found." });
        }
    }
}