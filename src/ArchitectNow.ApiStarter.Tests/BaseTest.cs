using System;
using System.IO;
using System.Reflection;
using ArchitectNow.ApiStarter.Api;
using ArchitectNow.ApiStarter.Api.Configuration;
using ArchitectNow.ApiStarter.Common;
using Autofac;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Module = Autofac.Module;

namespace ArchitectNow.ApiStarter.Tests
{
    public abstract class BaseTest
    {
        protected ILifetimeScope Scope { get; private set; }
        protected BaseTest()
        {
            Initialize();
        }

        private void Initialize()
        {
            var buildConfiguration = BuildConfiguration();
            Scope = BuildContainer(buildConfiguration);
        }

        protected IConfiguration BuildConfiguration()
        {
            var builder = new ConfigurationBuilder();
                
            var path = Path.GetFullPath(@"../../..");

            builder.SetBasePath(path).AddJsonFile("testsettings.json");

            var config = builder.Build();

            return config;
        }

        private IContainer BuildContainer(IConfiguration configuration)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();
            ConfigureLogger(serviceCollection);
            
            var modules = new Module[]
            {
                new CommonModule(),
                new ApiModule()
            };

            return serviceCollection.ConfigureAutofacContainer(configuration, b => { }, modules);
        }

        private void ConfigureLogger(ServiceCollection collection)
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
            
            collection.AddSingleton((ILoggerFactory) new SerilogLoggerFactory(Log.Logger, false));
        }
    }
}