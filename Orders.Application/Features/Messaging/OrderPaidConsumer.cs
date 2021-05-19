using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Messaging.Payments;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Orders.Application.Core;

namespace Orders.Application.Features.Messaging
{
    public class OrderPaidConsumer : IConsumer<OrderPaid>
    {
        private readonly IApplicationDbContext _context;

        public OrderPaidConsumer(IApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task Consume(ConsumeContext<OrderPaid> context)
        {
            var claim = await _context.Orders.SingleAsync(c => c.Id == context.Message.OrderId);

            if (claim.AmountDue - context.Message.AmountApplied >= 0) claim.AmountDue -= context.Message.AmountApplied;

            await _context.SaveChangesAsync(CancellationToken.None);
        }
    }
}