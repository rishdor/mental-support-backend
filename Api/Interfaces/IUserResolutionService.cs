using Api.Models;

namespace Api.Interfaces;

public interface IUserResolutionService
{
    Task<User> ResolveAsync(HttpContext context);
}