using Api.Data;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Services;

public class UserResolutionService
{
    private readonly AppDbContext _context;

    public UserResolutionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(User user, bool shouldShowOnboarding)> ResolveAsync(HttpContext context)
    {
        var firebaseUid = context.Items["FirebaseUid"] as string;

        if (string.IsNullOrWhiteSpace(firebaseUid))
        {
            throw new InvalidOperationException("Firebase UID not found in request context.");
        }

        var email = context.Items["FirebaseEmail"] as string;

        var user = await _context.Users
            .SingleOrDefaultAsync(u => u.FirebaseUid == firebaseUid);

        if (user == null)
        {
            user = new User
            {
                Id = Guid.NewGuid(),
                FirebaseUid = firebaseUid,
                Email = email,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return (user, true);
        }

        if (!string.IsNullOrWhiteSpace(email) && user.Email != email)
        {
            user.Email = email;
            await _context.SaveChangesAsync();
        }

        return (user, false);
    }
}
