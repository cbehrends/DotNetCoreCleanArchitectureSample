using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Payments.Application.Interfaces;
using Payments.Domain.Entities;

namespace Payments.Application.Features.Payments
{
    public static class ApplyPayment
    {

        public class Command : IRequest<Payment>
        {
            public int ClaimId { get; set; }
            public decimal PaymentAmount { get; set; }
        }
        
        public class Handler : IRequestHandler<Command, Payment>
        {
            private readonly IApplicationDbContext _context;
            
            public Handler(IApplicationDbContext context)
            {
                _context = context ?? throw new NullReferenceException(nameof(IApplicationDbContext));
            }
            public async Task<Payment> Handle(Command request, CancellationToken cancellationToken)
            {
                var newPayment = new Payment
                {
                    ClaimId = request.ClaimId,
                    PaymentAmount = request.PaymentAmount,
                    PaymentDate = DateTimeOffset.Now
                };
                
                _context.Payments.Add(newPayment);

                await _context.SaveChangesAsync(cancellationToken);
                
                return newPayment;
            }
        }
    }
}