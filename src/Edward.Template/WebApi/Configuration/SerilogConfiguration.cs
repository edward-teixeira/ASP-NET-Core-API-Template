namespace WebApi.Configuration
{
    using Serilog;

    public static class SerilogConfiguration
    {
        /// <summary>
        ///     Configures Serilog as the default log provider.
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <param name="builder"></param>
        public static IHostBuilder ConfigureLogger(this IHostBuilder hostBuilder, WebApplicationBuilder builder)
        {
            var configuredLogger = new Serilog.LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.WithProperty("Application", builder.Environment.ApplicationName)
                .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
                .Enrich.FromLogContext()
                .WriteTo.Conditional(
                    _ => builder.Environment.IsDevelopment(),
                    x => x.Console().WriteTo.Debug())
                .CreateLogger();

            return hostBuilder.UseSerilog(configuredLogger);
        }
    }
}