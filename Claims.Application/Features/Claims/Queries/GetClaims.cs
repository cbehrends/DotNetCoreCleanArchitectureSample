using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Claims.Application.Core;
using Claims.Application.Features.Claims.Model;
using Common.ApplicationCore.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Claims.Application.Features.Claims.Queries
{
    public static class GetClaims
    {
        public class Query : IRequest<List<ClaimsReadOnlyDto>>
        {
        }

        public class Handler : IRequestHandler<Query, List<ClaimsReadOnlyDto>>
        {
            private readonly IApplicationDbContext _context;

            public Handler(IApplicationDbContext context)
            {
                _context = context ?? throw new NullReferenceException(
                    "GetClaims Handler requires a non null IApplicationDbContext");
            }

            public async Task<List<ClaimsReadOnlyDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var retVal = await _context
                    .Claims
                    .Include(c => c.ServicesRendered)
                    .Select(c => new ClaimsReadOnlyDto
                        {Id = c.Id, FirstName = c.FirstName, ServicesRenderedCount = c.ServicesRendered.Count})
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                if (retVal.Count == 0) throw new NotFoundException("No Claims found");

                return retVal;
            }
        }
    }
}