using UserCrudApiChallenge.Domain.Entity;

namespace Auth.Demo
{
    public interface ITokenRefresher
    {
        AuthenticationResponse Refresh(RefreshCred refreshCred);
    }
}