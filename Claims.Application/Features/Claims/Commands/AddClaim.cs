using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Claims.Application.Interfaces;
using Claims.Domain.Entities;
using MediatR;

namespace Claims.Application.Features.Claims.Commands
{
    public static class AddClaim
    {
        public class Command: IRequest<Guid>
        {
            public string FirstName { get; set; }
            public List<Service> ServicesRendered { get; set; }
        }

        public class Handler : IRequestHandler<Command, Guid>
        {
            private IApplicationDbContext _context;

            public Handler(IApplicationDbContext context)
            {
                _context = context ?? throw new NullReferenceException("AddClaim Handler requires a non null IApplicationDbContext");
            }

            public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
            {
                var newClaim = new Claim
                {
                    FirstName = request.FirstName,
                    ServicesRendered = request.ServicesRendered
                };

                _context.Claims.Add(newClaim);
                await _context.SaveChangesAsync(cancellationToken);
                return newClaim.Id;
            }
        }
    }
}