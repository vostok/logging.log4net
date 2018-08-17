using System;
using JetBrains.Annotations;
using log4net.Core;
using log4net.Repository;
using log4net.Repository.Hierarchy;
using Vostok.Logging.Abstractions;
using ILog = Vostok.Logging.Abstractions.ILog;

namespace Vostok.Logging.Log4net
{
    /// <summary>
    /// <para>Represents an adapter between Vostok logging interfaces and log4net.</para>
    /// <para>It implements Vostok <see cref="ILog"/> interface using an externally provided instance of log4net <see cref="ILogger"/>.</para>
    /// <para>It does this by following these rules:</para>
    /// <list type="number">
    ///     <item><description>Vostok <see cref="LogLevel"/>s are directly translated to log4net <see cref="Level"/>s.<para/></description></item>
    ///     <item><description>Messages are prerendered into text as Vostok <see cref="ILog"/>'s formatting syntax differs from log4net.<para/></description></item>
    ///     <item><description>Properties are forwarded into log4net event's <see cref="LoggingEvent.Properties"/>.<para/></description></item>
    ///     <item><description><see cref="ForContext"/> with <c>null</c> argument returns a <see cref="Log4netLog"/> based on root logger from log4net's repository.<para/></description></item>
    ///     <item><description><see cref="ForContext"/> with non-<c>null</c> argument returns a <see cref="Log4netLog"/> based on logger with name equal to context value, obtained with <see cref="ILoggerRepository.GetLogger"/>.<para/></description></item>
    /// </list>
    /// </summary>
    public class Log4netLog : ILog
    {
        private readonly ILogger logger;

        public Log4netLog([NotNull] log4net.ILog log)
            : this(log.Logger)
        {
        }

        public Log4netLog([NotNull] ILogger logger)
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
            ILogger newLogger;

            if (context == null)
            {
                newLogger = (logger.Repository as Hierarchy)?.Root ?? logger;
            }
            else
            {
                newLogger = logger.Repository.GetLogger(context);
            }

            if (newLogger.Name == logger.Name)
                return this;

            return new Log4netLog(newLogger);
        }
    }
}