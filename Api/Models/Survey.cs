namespace Api.Models;

public class Survey
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string? UserId { get; set; }
    public string? ContentId { get; set; }

    public string ValueSignal { get; set; } = null!;
    public string ReturnIntent { get; set; } = null!;
    public string? Feedback { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}