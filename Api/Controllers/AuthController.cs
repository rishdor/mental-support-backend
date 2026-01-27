using Microsoft.AspNetCore.Mvc;
using Api.Mappers;
using Api.Interfaces;

namespace Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IUserResolutionService _userResolutionService;

    public AuthController(IUserResolutionService userResolutionService)
    {
        _userResolutionService = userResolutionService;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var user = await _userResolutionService.ResolveAsync(HttpContext);
        var response = user.ToResponse();

        return Ok(response);
    }
}