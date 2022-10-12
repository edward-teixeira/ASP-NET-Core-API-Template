namespace WebApi.Configuration
{
    using System.Reflection;
    using Microsoft.OpenApi.Models;

    public static class SwaggerConfiguration
    {
        /// <summary>
        ///     Configure Swagger
        /// </summary>
        /// <see>
        ///     <cref>https://aka.ms/aspnetcore/swashbuckle</cref>
        /// </see>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static IServiceCollection ConfigureSwagger(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddEndpointsApiExplorer();
            _ = services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = configuration["SwaggerUiOptions:Name"],
                        Description = configuration["SwaggerUiOptions:Description"],
                        Contact =
                            new OpenApiContact
                            {
                                Name = configuration["SwaggerUiOptions:ContactName"],
                                Email = configuration["SwaggerUiOptions:ContactEmail"],
                                Url = new Uri(configuration["SwaggerUiOptions:WebsiteUrl"])
                            },
                        License = new OpenApiLicense
                        {
                            Name = configuration["SwaggerUiOptions:LicenseName"],
                            Url = new Uri(configuration["SwaggerUiOptions:LicenseUrl"])
                        },
                        Version = "3.0.1"
                    });

                options.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        Description = "Add you JWT token: Bearer {token}",
                        Name = "Authorization",
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey
                    });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        Array.Empty<string>()
                    }
                });
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
            return services;
        }

        /// <summary>
        ///     SwaggerUi Middleware
        /// </summary>
        /// <param name="app"></param>
        public static void UseConfiguredSwaggerUi(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                //options.InjectStylesheet("/swagger-ui/custom.css");
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });
        }
    }
}