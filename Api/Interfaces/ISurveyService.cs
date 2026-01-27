using Api.Contracts;

namespace Api.Interfaces;

public interface ISurveyService
{
    Task LogSurveyResponse(Guid userId, SurveyRequest request);
}