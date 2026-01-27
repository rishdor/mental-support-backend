using Api.Contracts;
using Api.Data;
using Api.Models;
using Api.Interfaces;

namespace Api.Services;

public class SurveyService : ISurveyService
{
    private readonly AppDbContext _context;

    public SurveyService(AppDbContext context)
    {
        _context = context;
    }

    public async Task LogSurveyResponse(Guid userId, SurveyRequest request)
    {
        var survey = new Survey
        {
            UserId = userId,
            ContentItemId = request.ContentItemId,
            ValueSignal = request.ValueSignal,
            ReturnIntent = request.ReturnIntent,
            Feedback = request.Feedback,
            CreatedAt = DateTime.UtcNow
        };

        _context.Surveys.Add(survey);
        await _context.SaveChangesAsync();
    }
}