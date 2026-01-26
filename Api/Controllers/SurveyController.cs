using Microsoft.AspNetCore.Mvc;
using Api.Services;
using Api.Contracts;
namespace Api.Controllers;

[ApiController]
[Route("api/survey")]
public class SurveyController : ControllerBase
{
    private readonly SurveyService _surveyService;
    private readonly UserResolutionService _userResolver;

    public SurveyController(
        SurveyService surveyService,
        UserResolutionService userResolver)
    {
        _surveyService = surveyService;
        _userResolver = userResolver;
    }

    [HttpPost("submit")]
    public async Task<IActionResult> SubmitSurvey([FromBody] SurveyRequest request)
    {
        var user = await _userResolver.ResolveAsync(HttpContext);

        await _surveyService.LogSurveyResponse(user.Id, request);

        return NoContent();
    }
}