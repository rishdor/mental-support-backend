using Api.Models;
using Api.Contracts;

namespace Api.Mappers;

public static class UserMapper
{
    public static UserResponse ToResponse(this User user)
        => new UserResponse(
            user.Id,
            !user.HasCompletedOnboarding,
            user.Email
        );
}