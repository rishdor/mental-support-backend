using Microsoft.AspNetCore.Mvc;
using Api.Services;

namespace Api.Controllers;

[ApiController]
[Route("api/onboarding")]
public class OnboardingController : ControllerBase
{
    private readonly UserResolutionService _userResolver;
    private readonly OnboardingService _onboardingService;

    public OnboardingController(
        UserResolutionService userResolver,
        OnboardingService onboardingService)
    {
        _userResolver = userResolver;
        _onboardingService = onboardingService;
    }

    [HttpPost("complete")]
    public async Task<IActionResult> Complete()
    {
        var user = await _userResolver.ResolveAsync(HttpContext);

        await _onboardingService.CompleteOnboardingAsync(user);

        return NoContent();
    }
}
