

using System;
using System.Linq;
using System.Text.Unicode;
using System.Threading;
using System.Threading.Tasks;
using Claims.Application.Core;
using Claims.Application.Features.Messaging;
using MassTransit.Initializers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Claims.Application.Features.Claims.Commands
{
    public static class ApprovePayment
    {
        public class Command : IRequest
        {
            public int ClaimId { get;}

            public Command(int claimId)
            {
                ClaimId = claimId;
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
                    .Claims
                    .SingleAsync(c => c.Id == request.ClaimId, cancellationToken: cancellationToken)
                    .Select(c => c.AmountDue);
                
                await _publisher.SendClaimPaymentApproved(new ClaimPaymentApproved(request.ClaimId, paymentAmount));
                return Unit.Value;
            }
        }
        
    }
}