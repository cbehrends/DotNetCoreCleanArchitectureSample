using System;
using System.Threading;
using System.Threading.Tasks;
using Common.ApplicationCore.Exceptions;
using MediatR;
using Orders.Application.Core;
using Orders.Domain.Entities;

namespace Orders.Application.Features.Orders.Commands
{
    public static class DeleteOrder
    {
        public record Command : IRequest
        {
            public int Id { get; init; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly IApplicationDbContext _context;

            public Handler(IApplicationDbContext context)
            {
                _context = context ?? throw new NullReferenceException(
                    "DeleteOrder Handler requires a non null IApplicationDbContext");
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var claim = new Order {Id = request.Id};

                _context.Orders.Remove(claim);
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                }
                catch (Exception)
                {
                    throw new NotFoundException($"Order Id {request.Id} not found");
                }

                return Unit.Value;
            }
        }
    }
}