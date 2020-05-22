using System;
using Microsoft.Extensions.Logging;
using Moq;

namespace Claims.UnitTests.Behaviors
{
    public static class LoggingMockExtensions
    {
        public static void VerifyLogging<T>(this Mock<ILogger<T>> logger, string expectedMessage,
            LogLevel expectedLogLevel = LogLevel.Debug, Times? times = null)
        {
            times ??= Times.Once();

            Func<object, Type, bool> state = (v, t) => string.Compare(v.ToString(), expectedMessage, StringComparison.Ordinal) == 0;

            logger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == expectedLogLevel),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => state(v, t)),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), (Times) times);
        }

        public static void VerifyLogging<T>(this Mock<ILogger<T>> logger, LogLevel expectedLogLevel = LogLevel.Debug,
            Times? times = null)
        {
            times ??= Times.Once();

            Func<object, Type, bool> state = (v, t) => true;

            logger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == expectedLogLevel),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => state(v, t)),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), (Times) times);
        }
    }
}