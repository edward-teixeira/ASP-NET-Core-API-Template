namespace WebApi.Configuration
{
    using System.Reflection;
    using Microsoft.Extensions.Hosting;

    public static class HostConfiguration
    {
        /// <summary>
        ///     Sets up the configuration for the remainder of the build process and application. This can be called multiple times
        ///     and the results will be additive.
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <param name="args"></param>
        public static IHostBuilder ConfigureAppHost(this IHostBuilder hostBuilder,
            string[]? args)
        {
            hostBuilder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                var env = hostingContext.HostingEnvironment;
                // Reload app configuration with SelfHosted overrides.
                config.AddJsonFile("appsettings.json", true, true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);

                if (env.IsDevelopment())
                {
                    var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
                    config.AddUserSecrets(appAssembly, true);
                }

                config.AddEnvironmentVariables();
                _ = args != null ? config.AddCommandLine(args) : config;
            });
            return hostBuilder;
        }
    }
}