using System;
using System.Threading;
using System.Threading.Tasks;
using Claims.Application.Core;
using Common.Messaging.Payments;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Claims.Application.Features.Messaging
{
    public class ClaimPaidConsumer: IConsumer<ClaimPaid>
    {
        private IApplicationDbContext _context;

        public ClaimPaidConsumer(IApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(IApplicationDbContext));
        }
        public async Task Consume(ConsumeContext<ClaimPaid> context)
        {
            var claim = await _context.Claims.SingleAsync(c => c.Id == context.Message.ClaimId);

            if (claim.AmountDue - context.Message.AmountApplied >= 0)
            {
                claim.AmountDue -= context.Message.AmountApplied;
            }

            await _context.SaveChangesAsync(CancellationToken.None);
            
        }
    }
}