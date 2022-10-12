namespace WebApi.Middlewares
{
    using System.Net;
    using Microsoft.AspNetCore.Mvc;
    using WebApi.Exceptions;
    using WebApi.Extensions;
    using ValidationException = Exceptions.ValidationException;

    /// <summary>
    ///     Global exception handler middleware.
    ///     <seealso>
    ///         <cref>https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.middlewarefactory?view=aspnetcore-6.0</cref>
    ///     </seealso>
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly RequestDelegate _next;

        #region Ctor

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="environment"></param>
        /// <param name="next"></param>
        public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, IWebHostEnvironment environment,
            RequestDelegate next)
        {
            _logger = logger;
            _environment = environment;
            _next = next;
        }

        #endregion Ctor

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError("{ErrorMessage}", ex.Message);
                // Handle Error
                await HandleExceptionAsync(context, ex, _environment).ConfigureAwait(false);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex, IHostEnvironment env)
        {
            var statusCode = ex switch
            {
                AccessDeniedException => (int)HttpStatusCode.Forbidden,
                UnauthorizedException => (int)HttpStatusCode.Unauthorized,
                NotFoundException => (int)HttpStatusCode.NotFound,
                ValidationException => (int)HttpStatusCode.UnprocessableEntity,
                _ => (int)HttpStatusCode.InternalServerError
            };

            var shouldIncludeDetails = env.IsDevelopment();
            var title = shouldIncludeDetails ? "An error occured: " + ex.Message : "An error occured";
            var details = shouldIncludeDetails ? ex.ToStringFormatted() : ex.Message;

            var problem = new ProblemDetails { Status = statusCode, Detail = details, Title = title };
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsync(problem.JsonSerialize()!).ConfigureAwait(false);
        }
    }
}