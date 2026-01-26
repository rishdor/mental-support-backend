namespace Api.Contracts;

public class SurveyRequest
{
    public Guid ContentItemId { get; set; }
    public string ValueSignal { get; set; } = null!;
    public string ReturnIntent { get; set; } = null!;
    public string? Feedback { get; set; }
}