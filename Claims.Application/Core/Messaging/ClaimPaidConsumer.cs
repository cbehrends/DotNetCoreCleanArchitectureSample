using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Payments.Application.Core.Messaging;

namespace Claims.Application.Core.Messaging
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