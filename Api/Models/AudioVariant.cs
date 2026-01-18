namespace Api.Models;

public class AudioVariant
{
    public Guid Id { get; set; }
    public Guid ContentItemId { get; set; }
    public string EmotionalTone { get; set; } = null!;
    public string AudioUrl { get; set; } = null!;
    public int DurationSeconds { get; set; }

    public ContentItem ContentItem { get; set; } = null!;
}