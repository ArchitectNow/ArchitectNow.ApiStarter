using System.IO;
using System.Reflection;
using ArchitectNow.ApiStarter.Api;
using ArchitectNow.ApiStarter.Common;
using Autofac;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Module = Autofac.Module;

namespace ArchitectNow.ApiStarter.Tests
{
    public abstract class BaseTest
    {
        protected IConfiguration BuildConfiguration()
        {
            var builder = new ConfigurationBuilder();

            var path = Path.GetFullPath(@"../../..");

            builder.SetBasePath(path).AddJsonFile("testsettings.json");

            var config = builder.Build();

            return config;
        }

        public IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            builder.Register(ctx => BuildConfiguration()).As<IConfiguration>();

            var modules = new Module[]
            {
                new CommonModule(),
                new ApiModule()
            };

            foreach (var module in modules)
                builder.RegisterModule(module);

            //TODO:  Need to figure out how to register ILogger in an xUnit scenario
            //builder.RegisterType<ConsoleLogger>().As<Microsoft.Extensions.Logging.ILogger<User>>().SingleInstance();

            return builder.Build();
        }

        private void ConfigureLogger()
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
        }
    }
}