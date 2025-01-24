using ventry_api.Infrastructure.Security.Authentication.Entitites;

namespace ventry_api.Infrastructure.Security.Authentication.Application
{
    public interface ITokenService
    {
        string GenerateJwtToken(User user);
    }
}
