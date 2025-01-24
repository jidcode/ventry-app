using System.ComponentModel.DataAnnotations;

namespace ventry_api.Infrastructure.Security.Authentication.Entitites
{
    public class RegisterRequest
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Password { get; set; } = string.Empty;
    }
}
