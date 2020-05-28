using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Claims.Application.Core.Messaging;
using FluentAssertions;
using MassTransit;
using MassTransit.Testing;
using MassTransit.Turnout.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Payments.Application.Core.Interfaces;
using Payments.Application.Core.Messaging;
using Payments.Domain.Entities;

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
            var payments = new List<Payment>().AsQueryable();
            
            var paymentsMock = new Mock<DbSet<Payment>>();
            paymentsMock.As<IQueryable<Payment>>().Setup(m => m.Provider).Returns(payments.Provider);
            paymentsMock.As<IQueryable<Payment>>().Setup(m => m.Expression).Returns(payments.Expression);
            paymentsMock.As<IQueryable<Payment>>().Setup(m => m.ElementType).Returns(payments.ElementType);
            paymentsMock.As<IQueryable<Payment>>().Setup(m => m.GetEnumerator()).Returns(payments.GetEnumerator());

            mockContext.Setup(mc => mc.Payments).Returns(paymentsMock.Object);
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