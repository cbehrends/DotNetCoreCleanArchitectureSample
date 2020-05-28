

using System.Threading;
using System.Threading.Tasks;
using Claims.Application.Core.Messaging;
using MediatR;

namespace Claims.Application.Features.Claims.Commands
{
    public static class ApprovePayment
    {
        public class Command : IRequest
        {
            public int ClaimId { get; set; }
            public decimal PaymentAmount { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly IMessagePublisher _publisher;
            public Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                throw new System.NotImplementedException();
            }
        }
        
    }
}