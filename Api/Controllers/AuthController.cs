using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Api.Mappers;
using Api.Services;

namespace Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserResolutionService _userResolutionService;

    public AuthController(UserResolutionService userResolutionService)
    {
        _userResolutionService = userResolutionService;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var result = await _userResolutionService.ResolveAsync(HttpContext);
        var user = result.user.ToResponse();
        user.ShouldShowOnboarding = result.shouldShowOnboarding;

        return Ok(user);
    }
}