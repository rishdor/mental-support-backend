using Api.Data;
using Api.Models;

namespace Api.Services;

public class OnboardingService
{
    private readonly AppDbContext _context;

    public OnboardingService(AppDbContext context)
    {
        _context = context;
    }

    public async Task CompleteOnboardingAsync(User user)
    {
        if (user.HasCompletedOnboarding)
        {
            return;
        }

        user.HasCompletedOnboarding = true;
        await _context.SaveChangesAsync();
    }
}