using Api.Models;

namespace Api.Interfaces;

public interface IOnboardingService
{
    Task CompleteOnboardingAsync(User user);
}