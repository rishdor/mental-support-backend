using Api.Models;
using Api.Contracts;

namespace Api.Mappers;

public static class UserMapper
{
    public static UserResponse ToResponse(this User user)
        => new()
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty
        };
}