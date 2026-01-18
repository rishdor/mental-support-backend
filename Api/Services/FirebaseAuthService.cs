using FirebaseAdmin.Auth;

namespace Api.Services;
public class FirebaseAuthService
{
    public async Task<string> VerifyTokenAsync(string idToken)
    {
        var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
        return decodedToken.Uid;
    }
}
