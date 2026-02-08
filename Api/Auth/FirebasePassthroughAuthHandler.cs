using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

public class FirebasePassthroughAuthHandler
    : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public FirebasePassthroughAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder
    ) : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // user already set by FirebaseAuthMiddleware
        if (Context.User?.Identity?.IsAuthenticated == true)
        {
            return Task.FromResult(
                AuthenticateResult.Success(
                    new AuthenticationTicket(Context.User, Scheme.Name)
                )
            );
        }

        return Task.FromResult(AuthenticateResult.NoResult());
    }
}
