using System;
using Unclutter.SDK.Common;
using Unclutter.SDK.IServices;

namespace Unclutter.Services.Logging
{
    public class Logger : ILogger
    {
        /* Fields */
        private readonly Serilog.ILogger _logger;

        /* Constructors */
        public Logger(Serilog.ILogger logger)
        {
            _logger = logger;
        }

        /* Methods */
        public void Info(string messageTemplate, params object[] propertyValues) =>
            _logger.Information(messageTemplate, propertyValues);
        public void Error(Exception exception, string messageTemplate, params object[] propertyValues) =>
            _logger.Error(exception, messageTemplate, propertyValues);
        public void Debug(string messageTemplate, params object[] propertyValues) =>
                _logger.Debug(messageTemplate, propertyValues);
        public void Warning(string messageTemplate, params object[] propertyValues) =>
            _logger.Warning(messageTemplate, propertyValues);
    }
}
