using Microsoft.AspNetCore.Identity;
using ventry_api.Infrastructure.Security.Authentication.Entitites;

namespace ventry_api.Infrastructure.Security.Authentication.Application
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;

        public AuthRepository(
          UserManager<User> userManager,
          RoleManager<IdentityRole> roleManager,
          SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public async Task<User> FindByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<User> FindByUsername(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public async Task<IdentityResult> CreateUser(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<bool> CheckPassword(User user, string password)
        {
            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            return result.Succeeded;
        }

        public async Task<IdentityResult> AddToRole(User user, string roleName)
        {
            return await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<bool> RoleExists(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }

        public async Task<IdentityResult> CreateRole(string roleName)
        {
            return await _roleManager.CreateAsync(new IdentityRole(roleName));
        }

        public async Task<string> GenerateUniqueUsername(string firstName)
        {
            string baseUsername = firstName.ToLower().Substring(0, Math.Min(firstName.Length, 4));
            int remainingChars = 10 - baseUsername.Length;
            Random random = new Random();

            string username;
            do
            {
                string randomSuffix = new string(Enumerable.Repeat(0, remainingChars)
                  .Select(_ => (char)('0' + random.Next(10)))
                  .ToArray());
                username = $"{baseUsername}{randomSuffix}";
            } while (await FindByUsername(username) != null);

            return username;
        }

    }
}
