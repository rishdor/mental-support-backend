namespace Api.Contracts;

public record CreateContentRequest(
    string Title,
    string Description,
    string SituationTag,
    bool IsPremium,
    string AudioUrl,
    int DurationSeconds,
    string EmotionalTone
);