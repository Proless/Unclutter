using Serilog;
using Serilog.Formatting.Json;
using System;
using System.IO;
using Unclutter.SDK.IServices;
using ILogger = Unclutter.SDK.Common.ILogger;

namespace Unclutter.Services.Logging
{
    public class LoggerProvider : ILoggerProvider
    {
        private static ILogger _loggerInstance;
        public static ILogger Instance
        {
            get
            {
                if (_loggerInstance is null)
                {
                    _loggerInstance = CreateLogger(Path.Combine(Directory.GetCurrentDirectory(), "logs", "log.json"), 1048576, true);
                }
                return _loggerInstance;
            }
        }

        /* Fields */
        private readonly string _logsDirectory;

        /* Constructors */
        public LoggerProvider(IDirectoryService directoryService)
        {
            _logsDirectory = Path.Combine(directoryService.WorkingDirectory, "logs");

            directoryService.EnsureDirectoryAccess(_logsDirectory);
        }

        /* Methods */
        public ILogger GetInstance()
        {
            return Instance;
        }

        public ILogger GetLogger(string name)
        {
            try
            {
                var file = Path.Combine(_logsDirectory, $"{name}.json");
                return CreateLogger(file);
            }
            catch (Exception ex)
            {
                _loggerInstance.Error(ex, "Exception was thrown while creating a new ILogger with the name {0}", name);
                return null;
            }
        }

        /* Helpers */
        private static Logger CreateLogger(string file, long? fileSizeLimitBytes = null, bool shared = false)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(file) ?? string.Empty);
            }
            catch
            {
                // ignored
            }

            var fullPath = Path.GetFullPath(file);
            var config = new LoggerConfiguration()
                .WriteTo.File(new JsonFormatter(";", true), fullPath, rollOnFileSizeLimit: true, fileSizeLimitBytes: fileSizeLimitBytes, shared: shared)
                .MinimumLevel
                .Debug();

            return new Logger(config.CreateLogger(), fullPath);
        }
    }
}
