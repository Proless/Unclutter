using System;

namespace Unclutter.SDK.Common
{
    public interface ILogger
    {
        public string Location { get; }
        void Info(string messageTemplate, params object[] propertyValues);
        void Error(Exception exception, string messageTemplate, params object[] propertyValues);
        void Debug(string messageTemplate, params object[] propertyValues);
        void Warning(string messageTemplate, params object[] propertyValues);
    }
}
