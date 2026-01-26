namespace Api.Contracts;

public class SurveyRequest
{
    public string ContentId { get; set; } = null!;
    public string ValueSignal { get; set; } = null!;
    public string ReturnIntent { get; set; } = null!;
    public string? Feedback { get; set; }
}