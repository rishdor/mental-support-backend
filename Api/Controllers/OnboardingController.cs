using Microsoft.AspNetCore.Mvc;
using Api.Interfaces;

namespace Api.Controllers;

[ApiController]
[Route("api/onboarding")]
public class OnboardingController : ControllerBase
{
    private readonly IUserResolutionService _userResolver;
    private readonly IOnboardingService _onboardingService;

    public OnboardingController(
        IUserResolutionService userResolver,
        IOnboardingService onboardingService)
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
