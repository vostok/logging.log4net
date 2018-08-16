using System;
using JetBrains.Annotations;
using log4net.Core;
using Vostok.Logging.Abstractions;

namespace Vostok.Logging.Log4net
{
    // TODO(iloktionov): 1. xml-docs
    // TODO(iloktionov): 2. better unit test coverage
    // TODO(iloktionov): 3. correct ForContext() implementation

    public class Log4netHierarchicalLog : ILog
    {
        private readonly ILogger logger;
        
        public Log4netHierarchicalLog([NotNull] log4net.ILog log)
            : this(log.Logger)
        {
        }

        public Log4netHierarchicalLog([NotNull] ILogger logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
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