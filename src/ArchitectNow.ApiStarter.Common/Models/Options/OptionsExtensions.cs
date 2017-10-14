using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace ArchitectNow.ApiStarter.Common.Models.Options
{
    public static class OptionsExtensions
    {
        public static IOptions<TOptions> CreateOptions<TOptions>(this IConfiguration configuration, string section) where TOptions : class, new()
        {
            var options = configuration.GetSection(section).Get<TOptions>();

            return new OptionsWrapper<TOptions>(options);
        }
    }
}