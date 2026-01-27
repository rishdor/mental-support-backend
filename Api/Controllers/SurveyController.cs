using Microsoft.AspNetCore.Mvc;
using Api.Interfaces;
using Api.Contracts;

namespace Api.Controllers;

[ApiController]
[Route("api/survey")]
public class SurveyController : ControllerBase
{
    private readonly ISurveyService _surveyService;
    private readonly IUserResolutionService _userResolver;
    private readonly ILogger<SurveyController> _logger;

    public SurveyController(
        ISurveyService surveyService,
        IUserResolutionService userResolver,
        ILogger<SurveyController> logger)
    {
        _surveyService = surveyService;
        _userResolver = userResolver;
        _logger = logger;
    }

    [HttpPost("submit")]
    public async Task<IActionResult> SubmitSurvey([FromBody] SurveyRequest request)
    {
        var user = await _userResolver.ResolveAsync(HttpContext);

        await _surveyService.LogSurveyResponse(user.Id, request);

        _logger.LogInformation("Survey submission received for user {UserId}", user.Id);
        
        return NoContent();
    }
}