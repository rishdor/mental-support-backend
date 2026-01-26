using Api.Contracts;
using Api.Data;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Services;

public class SurveyService
{
    private readonly AppDbContext _context;

    public SurveyService(AppDbContext context)
    {
        _context = context;
    }

    public async Task LogSurveyResponse(string? userId, SurveyRequest request)
    {
        var survey = new Survey
        {
            UserId = userId,
            ContentId = request.ContentId,
            ValueSignal = request.ValueSignal,
            ReturnIntent = request.ReturnIntent,
            Feedback = request.Feedback,
            CreatedAt = DateTime.UtcNow
        };

        _context.Surveys.Add(survey);
        await _context.SaveChangesAsync();
    }
}