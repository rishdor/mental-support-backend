namespace Api.Contracts;

public record SurveyRequest(
    Guid ContentItemId,
    string ValueSignal,
    string ReturnIntent,
    string? Feedback
);