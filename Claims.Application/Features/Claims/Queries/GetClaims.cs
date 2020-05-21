using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Claims.Application.Interfaces;
using Claims.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Claims.Application.Features.Claims.Queries
{
    public static class GetClaims
    {
        public class Query: IRequest<List<Claim>>
        {
            
        }

        public class Handler : IRequestHandler<Query, List<Claim>>
        {
            private readonly IApplicationDbContext _context;

            public Handler(IApplicationDbContext context)
            {
                _context = context ?? throw new NullReferenceException("GetClaims Handler requires a non null IApplicationDbContext");
            }
            public async Task<List<Claim>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context
                    .Claims
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
            }
        }
    }
}