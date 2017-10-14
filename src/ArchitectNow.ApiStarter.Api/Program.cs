using System;
using System.IO;
using System.Reflection;
using Autofac.Extensions.DependencyInjection;
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
            {
                Directory.CreateDirectory(logPath);
            }

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
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables()
                .AddCommandLine(args);

            var configuration = builder.Build();

            return WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(configuration)
//                .UseEnvironment(configuration["environment:name"]) //TODO:  Read this from appsettings
                .UseEnvironment("development")
                .UseStartup<Startup>()
                .UseSerilog(Log.Logger)
                .Build();
        }
    }
}