namespace Api.Contracts;

public class ContentItemDetails
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string SituationTag { get; set; } = null!;
    public bool IsPremium { get; set; }
    public AudioVariantResponse? AudioVariant { get; set; } = new();
}