using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Api.Middleware;

public class FirebaseAuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<FirebaseAuthMiddleware> _logger;

    public FirebaseAuthMiddleware(RequestDelegate next, ILogger<FirebaseAuthMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

   public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        var allowAnonymous =
            endpoint?.Metadata.GetMetadata<IAllowAnonymous>() != null;

        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            if (allowAnonymous)
            {
                await _next(context);
                return;
            }

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new { error = "Unauthorized" });
            return;
        }

        var token = authHeader["Bearer ".Length..];

        try
        {
            var decoded =
                await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);

            context.Items["FirebaseUid"] = decoded.Uid;
            context.Items["FirebaseEmail"] =
                decoded.Claims.TryGetValue("email", out var email)
                    ? email?.ToString()
                    : null;

            _logger.LogInformation("Auth successful for UID: {Uid}", decoded.Uid);
            
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Auth failed: Invalid or expired token");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(
                new { error = "Invalid or expired token" }
            );
        }
    }
}
