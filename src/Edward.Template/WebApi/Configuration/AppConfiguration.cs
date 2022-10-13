namespace WebApi.Configuration
{
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Services;

    public static class AppConfiguration
    {
        public static IServiceCollection ConfigureAppServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(
                o => o.UseSqlServer(configuration.GetConnectionString("SqlServer"),
                    optionsBuilder => optionsBuilder.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

            services
                .AddScoped<DbContext>(provider => provider.GetRequiredService<AppDbContext>())
                .AddScoped<AppDbInitializer>()
                .AddScoped<ICacheService, CacheService>()
                .AddSingleton<IDateTimeService, DateTimeService>()
                ;

            return services;
        }
    }
}