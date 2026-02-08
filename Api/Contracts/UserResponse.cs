namespace Api.Contracts;
public record UserResponse(
    Guid Id,
    bool ShouldShowOnboarding,
    string? Email
);