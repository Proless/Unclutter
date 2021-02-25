#nullable enable
using Serilog;
using System;
using System.IO;
using Unclutter.SDK.IServices;
using ILogger = Unclutter.SDK.Common.ILogger;

namespace Unclutter.Services.Logging
{
    public class LoggerProvider : ILoggerProvider
    {
        /* Fields */
        private readonly ILogger _appLogger;
        private readonly string _logsDirectory;

        /* Constructors */
        public LoggerProvider(IDirectoryService directoryService)
        {
            _logsDirectory = Path.Combine(directoryService.WorkingDirectory, "logs");
            directoryService.EnsureDirectoryAccess(_logsDirectory);
            _appLogger = InternalCreateLogger(Path.Combine(_logsDirectory, "log.txt"), 1048576);
        }

        /* Methods */
        public ILogger GetAppLogger()
        {
            return _appLogger;
        }

        public ILogger? GetOrCreateLogger(string name)
        {
            try
            {
                var file = Path.Combine(_logsDirectory, $"{name}.txt");
                return InternalCreateLogger(file);
            }
            catch (Exception ex)
            {
                _appLogger.Error(ex, "Exception was thrown while creating a new ILogger with the name {0}", name);
                return null;
            }
        }

        /* Helpers */
        private Logger InternalCreateLogger(string file, long? fileSizeLimitBytes = null)
        {
            var config = new LoggerConfiguration()
                .WriteTo.File(file, rollOnFileSizeLimit: true, fileSizeLimitBytes: fileSizeLimitBytes)
                .MinimumLevel
                .Debug();

            return new Logger(config.CreateLogger());
        }
    }
}
