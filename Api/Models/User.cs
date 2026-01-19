namespace Api.Models;

public class User
{
    public Guid Id { get; set; }
    public string FirebaseUid { get; set; } = null!;
    public string? Email { get; set; }
    public DateTime CreatedAt { get; set; }
}