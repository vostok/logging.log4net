using System;
using log4net.Core;
using Vostok.Logging.Abstractions;

namespace Vostok.Logging.Log4net
{
    public class Log4netHierarchicalLog : ILog
    {
        private readonly ILogger logger;
        
        public Log4netHierarchicalLog(log4net.ILog log)
            : this(log.Logger)
        {
        }

        private Log4netHierarchicalLog(ILogger logger)
        {
            this.logger = logger;
        }

        public void Log(LogEvent @event)
        {
            if (@event == null)
                return;

            if (!IsEnabledFor(@event.Level))
                return;

            logger.Log(Log4netHelpers.TranslateEvent(logger, @event));
        }

        public bool IsEnabledFor(LogLevel level)
        {
            return logger.IsEnabledFor(Log4netHelpers.TranslateLevel(level));
        }

        public ILog ForContext(string context)
        {
            if (string.IsNullOrEmpty(context))
                throw new ArgumentException("Empty context is not allowed", nameof(context));
            var loggerName = $"{logger.Name}-{context}";
            return new Log4netHierarchicalLog(logger.Repository.GetLogger(loggerName));
        }
    }
}