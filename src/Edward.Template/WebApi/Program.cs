using WebApi.Configuration;
using WebApi.Data;
using WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Configuration for the remainder of the build process and application.
builder.Host
    .ConfigureAppHost(args)
    .ConfigureLogger(builder);

// Add services to the container.
builder.Services
    .ConfigureApiServices(builder.Configuration)
    .ConfigureAppServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // https://github.com/aspnet/Announcements/issues/432
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
    SeedDatabase(app).GetAwaiter().GetResult();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseConfiguredMiddlewares();

app.MapControllers();

app.Run();

static Task SeedDatabase(IHost app)
{
    using (var scope = app.Services.CreateScope())
    {
        var initializer = scope.ServiceProvider.GetRequiredService<AppDbInitializer>();
        ArgumentNullException.ThrowIfNull(initializer);
        return initializer.EarlyDevStageAsync();
    }
}