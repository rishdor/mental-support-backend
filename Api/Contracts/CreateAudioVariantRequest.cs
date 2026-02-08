namespace Api.Contracts;
public record CreateAudioVariantRequest(
    string AudioUrl,
    int DurationSeconds,
    string EmotionalTone
);