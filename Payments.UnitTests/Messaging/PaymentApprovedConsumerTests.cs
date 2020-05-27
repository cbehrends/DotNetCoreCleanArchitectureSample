using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Claims.Infrastructure.Messaging;
using FluentAssertions;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Payments.Infrastructure.Messaging;

namespace Payments.UnitTests.Messaging
{
    
    public class PaymentApprovedConsumerTests
    {

        [Test]
        public async Task Should_Respond_To_IClaimPaymentApproved_Event()
        {
            
            var harness = new InMemoryTestHarness();
            // var consumer = harness.Consumer<PaymentApprovedConsumer>();
            var mockLogger = new Mock<ILogger<PaymentApprovedConsumer>>();
            var consumer = harness.Consumer(() => new PaymentApprovedConsumer(mockLogger.Object));

            await harness.Start();
            try
            {
               
                var requestClient = await harness.ConnectRequestClient<IClaimPaymentApproved>();

                var response =  await requestClient.GetResponse<IMessageAccepted>(new MessageAccepted{Accepted = true});

                response.Message.Accepted.Should().BeTrue();

                Assert.That(consumer.Consumed.Select<IClaimPaymentApproved>().Any(), Is.True);

                Assert.That(harness.Sent.Select<IClaimPaymentApproved>().Any(), Is.True);
            }
            finally
            {
                await harness.Stop();
            }
            
            
        }
    }
}