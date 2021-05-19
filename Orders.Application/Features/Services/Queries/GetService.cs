using System;
using System.Threading;
using System.Threading.Tasks;
using Common.ApplicationCore.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Orders.Application.Core;
using Orders.Domain.Entities;

namespace Orders.Application.Features.Services.Queries
{
    public static class GetService
    {
        public class Query : IRequest<Service>
        {
            public int Id { get; init; }
        }

        public class Handler : IRequestHandler<Query, Service>
        {
            private readonly IApplicationDbContext _context;

            public Handler(IApplicationDbContext context)
            {
                _context = context ?? throw new NullReferenceException(
                    "GetOrder Handler requires a non null IApplicationDbContext");
            }

            public async Task<Service> Handle(Query request, CancellationToken cancellationToken)
            {
                var retVal = await _context
                    .Services
                    .SingleOrDefaultAsync(service => service.Id == request.Id, cancellationToken);

                if (retVal == null) throw new NotFoundException("Service", request.Id);

                return retVal;
            }
        }
    }
}