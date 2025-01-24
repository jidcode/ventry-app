using ventry_api.Infrastructure.Security.Authentication.Application;

namespace ventry_api.Shared.Extensions.ProgramServices
{
    public static class ApplicationExtension
    {
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthRepository, AuthRepository>();
        }
    }
}
