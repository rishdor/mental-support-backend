namespace Api.Models;

public class Survey
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ContentItemId { get; set; }

    public string ValueSignal { get; set; } = null!;
    public string ReturnIntent { get; set; } = null!;
    public string? Feedback { get; set; }

    public DateTime CreatedAt { get; set; }
}