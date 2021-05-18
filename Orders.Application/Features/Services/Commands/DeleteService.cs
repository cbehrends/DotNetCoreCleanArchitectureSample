using System;
using System.Threading;
using System.Threading.Tasks;
using Common.ApplicationCore.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Orders.Application.Core;

namespace Orders.Application.Features.Services.Commands
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
                var svcToDelete =
                    await _context
                        .Services
                        .AsNoTracking()
                        .SingleOrDefaultAsync(svc => svc.Id == request.Id, cancellationToken);

                if (svcToDelete == null) throw new NotFoundException($"Service {request.Id} not found");

                _context.Services.Remove(svcToDelete);

                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw new EntityInUseException(
                        $"{svcToDelete.Description} is linked to one or more claims and cannot be removed");
                }
                catch (DbUpdateException)
                {
                    throw new EntityInUseException(
                        $"{svcToDelete.Description} is linked to one or more claims and cannot be removed");
                }

                return Unit.Value;
            }
        }
    }
}