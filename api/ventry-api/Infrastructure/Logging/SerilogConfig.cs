using Serilog;

namespace ventry_api.Infrastructure.Logging
{
    public static class SerilogConfig
    {
        public static IHostBuilder ConfigureSerilog(this IHostBuilder host)
        {
            return host.UseSerilog((context, loggerConfig) =>
            {
                loggerConfig
                    .ReadFrom.Configuration(context.Configuration)
                    .Enrich.FromLogContext()
                    .Enrich.WithMachineName()
                    .Enrich.WithThreadId()
                    .Enrich.WithProperty("Application", "ventry-app")
                    .WriteTo.Seq("http://localhost:5341")
                    .WriteTo.Console();
            });
        }
    }
}
