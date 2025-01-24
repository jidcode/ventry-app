using Microsoft.AspNetCore.Identity;
using ventry_api.Infrastructure.Security.Authentication.Entitites;

namespace ventry_api.Infrastructure.Security.Authentication.Application
{
    public interface IAuthRepository
    {
        Task<User> FindByEmail(string email);

        Task<User> FindByUsername(string username);

        Task<IdentityResult> CreateUser(User user, string password);

        Task<bool> CheckPassword(User user, string password);

        Task<bool> RoleExists(string roleName);

        Task<IdentityResult> AddToRole(User user, string roleName);

        Task<IdentityResult> CreateRole(string roleName);

        Task<string> GenerateUniqueUsername(string firstName);

    }
}
