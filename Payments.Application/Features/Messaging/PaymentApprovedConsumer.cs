using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Messaging.Payments;
using MassTransit;
using Microsoft.Extensions.Logging;
using Payments.Application.Core.Interfaces;
using Payments.Domain.Entities;

namespace Payments.Application.Features.Messaging
{
    public class PaymentApprovedConsumer : IConsumer<IOrderPaymentApproved>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<PaymentApprovedConsumer> _logger;
        private readonly IPublishEndpoint _publishEndpoint;

        public PaymentApprovedConsumer(ILogger<PaymentApprovedConsumer> logger, IApplicationDbContext context,
            IPublishEndpoint publishEndpoint)
        {
            _logger = logger ?? throw new NullReferenceException(nameof(ILogger<PaymentApprovedConsumer>));
            _context = context ?? throw new NullReferenceException(nameof(IApplicationDbContext));
            _publishEndpoint = publishEndpoint ?? throw new NullReferenceException(nameof(IPublishEndpoint));
        }

        public async Task Consume(ConsumeContext<IOrderPaymentApproved> context)
        {
            var newPayment = new Payment
            {
                OrderId = context.Message.OrderId,
                PaymentAmount = context.Message.PaymentAmount,
                PaymentDate = DateTimeOffset.Now
            };

            _context.Payments.Add(newPayment);

            await _context.SaveChangesAsync(CancellationToken.None);

            await context.RespondAsync<IMessageAccepted>(new MessageAccepted {Accepted = true});

            var orderPaid = new OrderPaid
            {
                OrderId = context.Message.OrderId,
                AmountApplied = context.Message.PaymentAmount
            };
            _logger.LogInformation("Payment approved for order {OrderId}", context.Message.OrderId.ToString());
            await _publishEndpoint.Publish(orderPaid);
        }
    }
}