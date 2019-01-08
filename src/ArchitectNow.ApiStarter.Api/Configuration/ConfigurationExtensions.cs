using System;
using Microsoft.Extensions.Configuration;

namespace ArchitectNow.ApiStarter.Api.Configuration
{
    public static class ConfigurationExtensions
    {
        public static bool IsProduction(this IConfiguration configuration)
        {
            return configuration.InEnvironment("production");
        }

        public static bool IsDevelopment(this IConfiguration configuration)
        {
            return configuration.InEnvironment("development");
        }

        private static bool InEnvironment(this IConfiguration configuration, string environment)
        {
            var env = configuration["environment"];
            return string.Equals(env, environment, StringComparison.OrdinalIgnoreCase);
        }
    }
}