using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace ArchitectNow.ApiStarter.Api.Configuration
{
    public static class LoggingExtensions
    {
        public static LoggerConfiguration ConfigureLogging(this IConfiguration configuration,
            LoggerConfiguration loggerConfiguration = null)
        {
            var baseDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var logPath = Path.Combine(baseDir ?? string.Empty, "logs");
            if (!Directory.Exists(logPath)) Directory.CreateDirectory(logPath);

            var loggingConfiguration = loggerConfiguration ?? new LoggerConfiguration()
                                           .ReadFrom.Configuration(configuration)
                                           .WriteTo
                                           .RollingFile($@"{logPath}\{{Date}}.txt", retainedFileCountLimit: 10,
                                               shared: true)
                                           .WriteTo
                                           .Console(theme: AnsiConsoleTheme.Code);

            Log.Write(LogEventLevel.Information, "Logging has started");

            return loggingConfiguration;
        }
    }
}