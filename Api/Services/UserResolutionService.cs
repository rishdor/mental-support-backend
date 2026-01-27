using Api.Data;
using Api.Models;
using Microsoft.EntityFrameworkCore;
using Api.Interfaces;

namespace Api.Services;

public class UserResolutionService : IUserResolutionService
{
    private readonly AppDbContext _context;

    public UserResolutionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User> ResolveAsync(HttpContext context)
    {
        var firebaseUid = context.Items["FirebaseUid"] as string;

        if (string.IsNullOrWhiteSpace(firebaseUid))
        {
            throw new InvalidOperationException("Firebase UID not found in request context.");
        }

        var email = context.Items["FirebaseEmail"] as string;
        var isAnonymous = string.IsNullOrWhiteSpace(email);

        var user = await _context.Users
            .SingleOrDefaultAsync(u => u.FirebaseUid == firebaseUid);

        if (user == null)
        {
            user = new User
            {
                Id = Guid.NewGuid(),
                FirebaseUid = firebaseUid,
                Email = email,
                IsAnonymous = isAnonymous,
                HasCompletedOnboarding = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        var needsUpdate = false;

        if (user.Email != email)
        {
            user.Email = email;
            needsUpdate = true;
        }

        if (user.IsAnonymous != isAnonymous)
        {
            user.IsAnonymous = isAnonymous;
            needsUpdate = true;
        }

        if (needsUpdate)
        {
            await _context.SaveChangesAsync();
        }

        return user;
    }
}