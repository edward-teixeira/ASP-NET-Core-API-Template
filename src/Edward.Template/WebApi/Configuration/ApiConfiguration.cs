namespace WebApi.Configuration
{
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Microsoft.AspNetCore.Mvc;
    using Serilog;

    public static class ApiConfiguration
    {
        public static IServiceCollection ConfigureApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Disable default ModelState validation behavior
            // See more https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.apibehavioroptions
            services
                .Configure<ApiBehaviorOptions>(
                    o => o.SuppressModelStateInvalidFilter = true);
            services
                // https://github.com/aspnet/Announcements/issues/432
                .AddDatabaseDeveloperPageExceptionFilter()
                .AddHttpContextAccessor()
                .AddMemoryCache()
                .ConfigureCors()
                .ConfigureResponseCompression(configuration);

            services
                .AddControllers(
                o => o.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true)
                .AddJsonOptions(jsonOptions =>
                {
                    jsonOptions.JsonSerializerOptions.WriteIndented = true;
                    jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    jsonOptions.JsonSerializerOptions.NumberHandling = JsonNumberHandling.Strict;
                })
                .AddXmlSerializerFormatters();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.ConfigureSwagger(configuration);

            return services;
        }

        /// <summary>
        ///     Configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        public static void UseConfiguredMiddlewares(this WebApplication app)
        {
            if (!app.Environment.IsDevelopment())
            {
                // Forwarded Headers Middleware should run before other middleware.
                app.UseForwardedHeaders();
                // see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // https://github.com/serilog/serilog-aspnetcore#request-logging
            app.UseSerilogRequestLogging();

            app.UseConfiguredSwaggerUi();
            app.UseConfiguredCors();

            // Authentication and Authorization
            //app.UseAuthentication();
            app.UseAuthorization();

            // Adds middleware for dynamically compressing HTTP Responses.
            app.UseResponseCompression();
            app.UseResponseCaching();
        }
    }
}