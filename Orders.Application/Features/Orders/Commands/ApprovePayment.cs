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
            public int OrderId { get;}

            public Command(int orderId)
            {
                OrderId = orderId;
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly IMessagePublisher _publisher;
            public readonly IApplicationDbContext _context;
            public Handler(IMessagePublisher publisher, IApplicationDbContext context)
            {
                _context = context ?? throw new ArgumentNullException(nameof(IApplicationDbContext)); 
                _publisher = publisher ?? throw new ArgumentNullException(nameof(IMessagePublisher));
            }
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var paymentAmount = await _context
                    .Orders
                    .SingleAsync(c => c.Id == request.OrderId, cancellationToken: cancellationToken)
                    .Select(c => c.AmountDue);
                
                await _publisher.SendClaimPaymentApproved(new OrderPaymentApproved(request.OrderId, paymentAmount));
                return Unit.Value;
            }
        }
        
    }
}