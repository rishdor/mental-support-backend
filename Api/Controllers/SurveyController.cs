using Microsoft.AspNetCore.Mvc;
using Api.Services;
using Api.Contracts;

namespace Api.Controllers;

[ApiController]
[Route("api/survey")]
public class SurveyController : ControllerBase
{
    private readonly SurveyService _surveyService;

    public SurveyController(
        SurveyService surveyService)
    {
        _surveyService = surveyService;
    }

    [HttpPost]
    public async Task<IActionResult> SubmitSurvey( [FromBody] SurveyRequest request, HttpContext context)
    {
        if (string.IsNullOrWhiteSpace(request.ContentId))
        {
            return BadRequest("ContentId is required");
        }

        var userId = context.Items["FirebaseUid"] as string;

        await _surveyService.LogSurveyResponse(userId, request);

        return NoContent();
    }
}