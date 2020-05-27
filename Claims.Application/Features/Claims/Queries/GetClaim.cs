using System;
using System.Threading;
using System.Threading.Tasks;
using Claims.Application.Core;
using Claims.Domain.Entities;
using Common.ApplicationCore.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Claims.Application.Features.Claims.Queries
{
    public static class GetClaim
    {
        public class Query : IRequest<Claim>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Claim>
        {
            private readonly IApplicationDbContext _context;

            public Handler(IApplicationDbContext context)
            {
                _context = context ?? throw new NullReferenceException(
                    "GetClaim Handler requires a non null IApplicationDbContext");
            }

            public async Task<Claim> Handle(Query request, CancellationToken cancellationToken)
            {
                var retVal = await _context
                    .Claims
                    .Include(claim => claim.ServicesRendered)
                    .ThenInclude(sr => sr.Service)
                    .SingleOrDefaultAsync(claim => claim.Id == request.Id, cancellationToken);

                if (retVal == null) throw new NotFoundException("Claim", request.Id);

                return retVal;
            }
        }
    }
}