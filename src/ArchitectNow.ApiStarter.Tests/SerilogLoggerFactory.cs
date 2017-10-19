using Microsoft.Extensions.Logging;
using Serilog.Debugging;
using Serilog.Extensions.Logging;
using ILogger = Serilog.ILogger;

namespace ArchitectNow.ApiStarter.Tests
{
    internal class SerilogLoggerFactory : ILoggerFactory
    {
        private readonly SerilogLoggerProvider _provider;

        public SerilogLoggerFactory(ILogger logger = null, bool dispose = false)
        {
            _provider = new SerilogLoggerProvider(logger, dispose);
        }

        public void Dispose()
        {
            _provider.Dispose();
        }

        public Microsoft.Extensions.Logging.ILogger CreateLogger(string categoryName)
        {
            return _provider.CreateLogger(categoryName);
        }

        public void AddProvider(ILoggerProvider provider)
        {
            SelfLog.WriteLine("Ignoring added logger provider {0}", provider);
        }
    }
}