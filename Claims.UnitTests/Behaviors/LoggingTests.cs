using System;
using System.Threading;
using System.Threading.Tasks;
using Claims.Application.Core.Behaviours;
using Claims.Application.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Claims.UnitTests.Behaviors
{
    public class LoggingTests
    {
        private Mock<ILogger<MockRequest>> _loggingMock = new Mock<ILogger<MockRequest>>();
        private Mock<ICurrentUserService> _currentUserServiceMock = new Mock<ICurrentUserService>();

        [Test]
        public async Task Should_Log_All_Requests_At_Info_Level()
        {
            var mockReq = new MockRequest();
            _loggingMock = new Mock<ILogger<MockRequest>>();
            _currentUserServiceMock.Setup(cus => cus.UserId).Returns("corey");

            var sut = new LoggingBehaviour<MockRequest>(_loggingMock.Object, _currentUserServiceMock.Object);
            await sut.Process(mockReq, CancellationToken.None);

            _loggingMock.VerifyLogging(
                "Request: MockRequest corey Placeholder Name Claims.UnitTests.Behaviors.MockRequest",
                LogLevel.Information);
        }

        [Test]
        public async Task Should_Log_A_Warning_For_Slow_Requests()
        {
            var mockReq = new MockRequest();
            _currentUserServiceMock.Setup(cus => cus.UserId).Returns("corey");

            var sut = new PerformanceBehaviour<MockRequest, bool>(_loggingMock.Object, _currentUserServiceMock.Object);
            await sut.Handle(mockReq, CancellationToken.None, async () =>
            {
                Thread.Sleep(501);
                return true;
            });

            _loggingMock.VerifyLogging(LogLevel.Warning);
        }

        [Test]
        public async Task Should_Log_A_Exception_For_UnhandledException()
        {
            var mockReq = new MockRequest();
            _currentUserServiceMock.Setup(cus => cus.UserId).Returns("corey");

            var sut = new UnhandledExceptionBehaviour<MockRequest, bool>(_loggingMock.Object);

            // Make a successful call to ensure coverage
            await sut.Handle(mockReq, CancellationToken.None, async () => true);

            try
            {
                // Now we can blow it up
                await sut.Handle(mockReq, CancellationToken.None, () => throw new Exception("BOOOM"));
            }
            catch (Exception)
            {
                //Ignore error
            }

            _loggingMock.VerifyLogging(LogLevel.Error);
        }
    }
}