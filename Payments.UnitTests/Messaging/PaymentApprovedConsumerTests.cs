using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Claims.Application.Core.Messaging;
using FluentAssertions;
using MassTransit;
using MassTransit.Testing;
using MassTransit.Turnout.Contracts;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Payments.Application.Core.Interfaces;
using Payments.Application.Core.Messaging;

namespace Payments.UnitTests.Messaging
{
    
    public class PaymentApprovedConsumerTests
    {

        [Test]
        public async Task Should_Respond_To_IClaimPaymentApproved_Event()
        {
            
            var harness = new InMemoryTestHarness();
            var mockContext = new Mock<IApplicationDbContext>();
            var mockLogger = new Mock<ILogger<PaymentApprovedConsumer>>();
            var mockEndpoint = new Mock<IPublishEndpoint>();
            var consumer = harness.Consumer(() => new PaymentApprovedConsumer(mockLogger.Object, mockContext.Object, mockEndpoint.Object));

            await harness.Start();
            
            try
            {
               
                var requestClient = await harness.ConnectRequestClient<IClaimPaymentApproved>();

                var response =  await requestClient.GetResponse<IMessageAccepted>(new MessageAccepted{Accepted = true});

                response.Message.Accepted.Should().BeTrue();

                Assert.That(consumer.Consumed.Select<IClaimPaymentApproved>().Any(), Is.True);

                Assert.That(harness.Sent.Select<IClaimPaymentApproved>().Any(), Is.True);
                mockContext.Verify(mc => 
                    mc.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            }
            finally
            {
                await harness.Stop();
            }
            
        }
    }
}