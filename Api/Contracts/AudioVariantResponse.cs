namespace Api.Contracts;

public record AudioVariantResponse(
    Guid Id,
    string EmotionalTone,
    string AudioUrl,
    int DurationSeconds
);