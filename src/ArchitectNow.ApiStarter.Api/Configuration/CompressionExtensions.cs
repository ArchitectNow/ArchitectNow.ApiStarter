using System.IO.Compression;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;

namespace ArchitectNow.ApiStarter.Api.Configuration
{
    public static class CompressionExtensions
    {
        public static void ConfigureCompression(this IServiceCollection services)
        {
            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);
            services.AddResponseCompression(options => { options.Providers.Add<GzipCompressionProvider>(); });
        }

        public static void ConfigureCompression(this IApplicationBuilder app)
        {
            app.UseResponseCompression();
        }
    }
}