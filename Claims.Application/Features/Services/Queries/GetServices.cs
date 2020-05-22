using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Claims.Application.Core.Exceptions;
using Claims.Application.Core.Interfaces;
using Claims.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Claims.Application.Features.Services.Queries
{
    public static class GetServices
    {
        public class Query: IRequest<List<Service>>
        {
            
        }

        public class Handler : IRequestHandler<Query, List<Service>>
        {
            private readonly IApplicationDbContext _context;

            public Handler(IApplicationDbContext context)
            {
                _context = context ?? throw new NullReferenceException("GetServices Handler requires a non null IApplicationDbContext");
            }
            public async Task<List<Service>> Handle(Query request, CancellationToken cancellationToken)
            {
                var retVal = await _context
                    .Services
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                if (retVal.Count == 0)
                {
                    throw new NotFoundException("No Services found");
                }

                return retVal;
            }
        }
    }
}