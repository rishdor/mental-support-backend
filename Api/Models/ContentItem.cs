namespace Api.Models;

public class ContentItem
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string SituationTag { get; set; } = null!;
    public bool IsPremium { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<AudioVariant> AudioVariants { get; set; } = new List<AudioVariant>();
}