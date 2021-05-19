using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit.Initializers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Orders.Application.Core;
using Orders.Application.Features.Messaging;

namespace Orders.Application.Features.Orders.Commands
{
    public static class ApprovePayment
    {
        public class Command : IRequest
        {
            public Command(int orderId)
            {
                OrderId = orderId;
            }

            public int OrderId { get; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMessagePublisher _publisher;

            public Handler(IMessagePublisher publisher, IApplicationDbContext context)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
                _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var paymentAmount = await _context
                    .Orders
                    .SingleAsync(c => c.Id == request.OrderId, cancellationToken)
                    .Select(c => c.AmountDue);

                await _publisher.SendClaimPaymentApproved(new OrderPaymentApproved(request.OrderId, paymentAmount));
                return Unit.Value;
            }
        }
    }
}