using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace ArchitectNow.ApiStarter.Api
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var baseDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var logPath = Path.Combine(baseDir, "logs");
            if (!Directory.Exists(logPath))
                Directory.CreateDirectory(logPath);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo
                .RollingFile($@"{logPath}\{{Date}}.txt", retainedFileCountLimit: 10, shared: true)
                .WriteTo.ColoredConsole()
                .CreateLogger();

            try
            {
                BuildWebHost(args)
                    .Run();
                return 0;
            }
            catch (Exception exception)
            {
                Log.Fatal(exception, "Site terminated");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IWebHost BuildWebHost(string[] args)
        {
            //pulls in environment from cli args or env vars
            var builder = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddCommandLine(args);

            var configuration = builder.Build();

            //https://github.com/aspnet/MetaPackages/blob/633cb681493c0958a9d215624c173db29e20c23d/src/Microsoft.AspNetCore/WebHost.cs

            return WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(configuration)
                .UseStartup<Startup>()
                .UseSerilog(Log.Logger)
                .Build();
        }
    }
}