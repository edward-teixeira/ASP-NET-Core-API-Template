namespace WebApi.Configuration
{
    /// <summary>
    ///     Cors configuration middleware.
    /// </summary>
    public static class CorsConfiguration
    {
        /// <summary>
        ///     Adds cross-origin resource sharing services to the specified IServiceCollection.
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(opt => opt
                .AddPolicy(
                    "MyCorsPolicy",
                    configurePolicyBuilder
                        => configurePolicyBuilder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                ));

            return services;
        }

        /// <summary>
        ///     Adds a CORS middleware to your web application pipeline to allow cross domain requests.
        /// </summary>
        /// <param name="app"></param>
        public static IApplicationBuilder UseConfiguredCors(this IApplicationBuilder app)
            => app.UseCors("MyCorsPolicy");
    }
}