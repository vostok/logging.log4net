using System;
using System.Collections.Generic;
using log4net.Core;
using log4net.Repository.Hierarchy;
using log4net.Util;
using Vostok.Logging.Abstractions;
using Vostok.Logging.Formatting;

namespace Vostok.Logging.Log4net
{
    public static class Log4netHelpers
    {
        public static Level TranslateLevel(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    return Level.Debug;
                case LogLevel.Info:
                    return Level.Info;
                case LogLevel.Warn:
                    return Level.Warn;
                case LogLevel.Error:
                    return Level.Error;
                case LogLevel.Fatal:
                    return Level.Fatal;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }
        }

        public static LoggingEvent TranslateEvent(ILogger logger, LogEvent @event)
        {
            var message = LogEventFormatter.Format(@event, OutputTemplate.Default);
            var properties = TranslateProperties(@event.Properties);
            var loggingEventData = new LoggingEventData
            {
                Level = TranslateLevel(@event.Level),
                Properties = properties,
                TimeStampUtc = @event.Timestamp.UtcDateTime,
                ExceptionString = @event.Exception == null ? null : logger.Repository.RendererMap.FindAndRender(@event.Exception),
                Message = logger.Repository.RendererMap.FindAndRender(message),
                LoggerName = logger.Name
            };
            return new LoggingEvent(typeof(Logger), logger.Repository, loggingEventData);
        }

        private static PropertiesDictionary TranslateProperties(IReadOnlyDictionary<string, object> readOnlyDictionary)
        {
            if (readOnlyDictionary == null)
                return null;

            var properties = new PropertiesDictionary();
            foreach (var kvp in readOnlyDictionary)
                properties[kvp.Key] = kvp.Value;
            return properties;
        }
    }
}