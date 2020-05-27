using System;
using System.Threading;
using System.Threading.Tasks;
using Claims.Application.Core;
using Claims.Domain.Entities;
using Common.ApplicationCore.Exceptions;
using MediatR;

namespace Claims.Application.Features.Claims.Commands
{
    public static class DeleteClaim
    {
        public class Command : IRequest
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly IApplicationDbContext _context;

            public Handler(IApplicationDbContext context)
            {
                _context = context ?? throw new NullReferenceException(
                    "DeleteClaim Handler requires a non null IApplicationDbContext");
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var claim = new Claim {Id = request.Id};

                _context.Claims.Remove(claim);
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                }
                catch (Exception)
                {
                    throw new NotFoundException($"Claim Id {request.Id} not found");
                }

                return Unit.Value;
            }
        }
    }
}