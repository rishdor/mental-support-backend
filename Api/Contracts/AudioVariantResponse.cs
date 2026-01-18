namespace Api.Contracts;

public class AudioVariantResponse
{
    public Guid Id { get; set; }
    public string EmotionalTone { get; set; } = null!;
    public string AudioUrl { get; set; } = null!;
    public int DurationSeconds { get; set; }
}