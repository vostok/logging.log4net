using System;
using System.Linq;
using FluentAssertions;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Vostok.Logging.Log4net.Tests
{
    [TestFixture]
    internal class Log4netLog_Tests
    {
        [TestCase("")]
        [TestCase(null)]
        public void Log4netLog_should_throw_ArgumentException_on_empty_context(string context)
        {
            Assert.Throws<ArgumentException>(() => log.ForContext(context));
        }

        [Test]
        public void Log4netLog_should_log_messages()
        {
            var messages = new[] { "Hello, World 1", "Hello, World 2" };
            
            log.Info(messages[0]);
            log.Info(messages[1]);

            appender.GetEvents().Select(x => x.RenderedMessage).Should().Equal(messages);
        }

        [Test]
        public void Log4netLog_should_use_context_as_logger_name()
        {
            var contexts = new[] { "lalala", "bububu" };
            log.ForContext(contexts[0]).Info("msg1");
            log.ForContext(contexts[1]).Info("msg2");
            appender.GetEvents().Select(x => x.LoggerName).Should().Equal(contexts);
        }

        [Test]
        public void Log4netLog_should_create_events_with_correct_timestamp()
        {
            var timestamp = DateTimeOffset.UtcNow.AddDays(1);
            log.Log(new LogEvent(LogLevel.Info, timestamp, "lalala"));
            appender.GetEvents().Single().TimeStampUtc.Should().Be(timestamp.UtcDateTime);
        }

        [TestCaseSource(nameof(GetLevelsMap))]
        public void Log4netLog_should_translate_level_correctly(LogLevel level, Level log4netLevel)
        {
            log.Log(new LogEvent(level, DateTimeOffset.UtcNow, "lalala"));
            appender.GetEvents().Single().Level.Should().Be(log4netLevel);
        }

        private static object[][] GetLevelsMap()
        {
            return new[]
            {
                new object[] {LogLevel.Fatal, Level.Fatal},
                new object[] {LogLevel.Error, Level.Error},
                new object[] {LogLevel.Warn, Level.Warn},
                new object[] {LogLevel.Info, Level.Info},
                new object[] {LogLevel.Debug, Level.Debug},
            };
        }

        [SetUp]
        public void SetUp()
        {
            var repository = LogManager.GetAllRepositories().SingleOrDefault(x => x.Name == "test") ?? LogManager.CreateRepository("test");
            repository.ResetConfiguration();
            appender = new MemoryAppender();
            BasicConfigurator.Configure(repository, appender);
            log = new Log4netLog(LogManager.GetLogger("test", "root"));
        }

        private MemoryAppender appender;
        private Abstractions.ILog log;
    }
}