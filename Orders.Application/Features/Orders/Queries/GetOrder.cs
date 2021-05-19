using System;
using System.Threading;
using System.Threading.Tasks;
using Common.ApplicationCore.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Orders.Application.Core;
using Orders.Domain.Entities;

namespace Orders.Application.Features.Orders.Queries
{
    public static class GetOrder
    {
        public class Query : IRequest<Order>
        {
            public int Id { get; init; }
        }

        public class Handler : IRequestHandler<Query, Order>
        {
            private readonly IApplicationDbContext _context;

            public Handler(IApplicationDbContext context)
            {
                _context = context ?? throw new NullReferenceException(
                    "GetOrder Handler requires a non null IApplicationDbContext");
            }

            public async Task<Order> Handle(Query request, CancellationToken cancellationToken)
            {
                var retVal = await _context
                    .Orders
                    .Include(claim => claim.ServicesRendered)
                    .ThenInclude<Order, RenderedService, Service>(sr => sr.Service)
                    .SingleOrDefaultAsync(claim => claim.Id == request.Id, cancellationToken);

                if (retVal == null) throw new NotFoundException("Order", request.Id);

                return retVal;
            }
        }
    }
}