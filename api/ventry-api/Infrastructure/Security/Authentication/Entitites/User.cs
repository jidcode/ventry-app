using Microsoft.AspNetCore.Identity;

namespace ventry_api.Infrastructure.Security.Authentication.Entitites
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
