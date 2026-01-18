namespace Api.Models;

public class User
{
    public Guid Id { get; set; }
    public string FirebaseUid { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}