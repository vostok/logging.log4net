using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using log4net.Core;
using log4net.Repository.Hierarchy;
using log4net.Util;
using Vostok.Logging.Abstractions;
using Vostok.Logging.Formatting;

namespace Vostok.Logging.Log4net
{
    internal static class Log4netHelpers
    {
        [NotNull]
        public static LoggingEvent TranslateEvent([NotNull] ILogger logger, [NotNull] LogEvent @event)
        {
            var level = TranslateLevel(@event.Level);
            var properties = TranslateProperties(@event.Properties);
            var message = LogMessageFormatter.Format(@event);
            var timestamp = @event.Timestamp.UtcDateTime;

            var loggingEventData = new LoggingEventData
            {
                Level = level,
                Message = message,
                Properties = properties,
                TimeStampUtc = timestamp,
                LoggerName = logger.Name
            };

            // TODO(iloktionov): set exception without rendering it
            var loggingEvent = new LoggingEvent(typeof(Logger), logger.Repository, loggingEventData, default);

            return loggingEvent;
        }

        [NotNull]
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
                    throw new ArgumentOutOfRangeException(nameof(level), level, $"Unexpected value of {nameof(LogLevel)}.");
            }
        }

        [CanBeNull]
        private static PropertiesDictionary TranslateProperties([CanBeNull] IReadOnlyDictionary<string, object> properties)
        {
            if (properties == null || properties.Count == 0)
                return null;

            var result = new PropertiesDictionary();

            foreach (var pair in properties)
                result[pair.Key] = pair.Value;

            return result;
        }
    }
}