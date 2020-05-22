using System;
using System.Threading;
using System.Threading.Tasks;
using Claims.Application.Core.Exceptions;
using Claims.Application.Core.Interfaces;
using Claims.Domain.Entities;
using MediatR;

namespace Claims.Application.Features.Services.Commands
{
    public static class DeleteService
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
                _context = context ?? throw new NullReferenceException(nameof(IApplicationDbContext));
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var svc = new Service {Id = request.Id};

                _context.Services.Remove(svc);
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                }
                catch (Exception)
                {
                    throw new NotFoundException($"Service {request.Id} not found or is linked to one or more claims");
                }

                return Unit.Value;
            }
        }
    }
}