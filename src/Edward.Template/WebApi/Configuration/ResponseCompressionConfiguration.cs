namespace WebApi.Configuration
{
    using System.IO.Compression;
    using Microsoft.AspNetCore.ResponseCompression;

    /// <summary>
    ///     Add response compression services.
    /// </summary>
    public static class ResponseCompressionConfiguration
    {
        /// <summary>
        ///     Configures middleware for dynamically compressing HTTP Responses.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        public static IServiceCollection ConfigureResponseCompression(this IServiceCollection services,
            IConfiguration config)
        {
            services.AddResponseCompression(options =>
            {
                options.MimeTypes = config.Get<ResponseCompressionOptions>().MimeTypes
                    .Concat(ResponseCompressionDefaults.MimeTypes);
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
            });
            services.Configure<BrotliCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);
            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);
            return services;
        }
    }
}