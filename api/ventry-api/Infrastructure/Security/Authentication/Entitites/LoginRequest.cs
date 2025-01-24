using System.ComponentModel.DataAnnotations;

namespace ventry_api.Infrastructure.Security.Authentication.Entitites
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
