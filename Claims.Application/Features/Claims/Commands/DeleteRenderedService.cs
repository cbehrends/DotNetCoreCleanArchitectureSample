using System;
using System.Threading;
using System.Threading.Tasks;
using Claims.Application.Core.Exceptions;
using Claims.Application.Core.Interfaces;
using Claims.Domain.Entities;
using MediatR;

namespace Claims.Application.Features.Claims.Commands
{
    public static class DeleteRenderedService
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
                var renderedSvcToDelete = new RenderedService {Id = request.Id};

                _context.RenderedServices.Remove(renderedSvcToDelete);
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                }
                catch (Exception)
                {
                    throw new NotFoundException($"RenderedService Id {request.Id} not found");
                }

                return Unit.Value;
            }
        }
    }
}