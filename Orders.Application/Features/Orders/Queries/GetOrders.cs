using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.ApplicationCore.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Orders.Application.Core;
using Orders.Application.Features.Orders.Model;

namespace Orders.Application.Features.Orders.Queries
{
    public static class GetOrders
    {
        public class Query : IRequest<List<OrderReadOnlyDto>>
        {
        }

        public class Handler : IRequestHandler<Query, List<OrderReadOnlyDto>>
        {
            private readonly IApplicationDbContext _context;

            public Handler(IApplicationDbContext context)
            {
                _context = context ?? throw new NullReferenceException(
                    "GetOrders Handler requires a non null IApplicationDbContext");
            }

            public async Task<List<OrderReadOnlyDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var retVal = await _context
                    .Orders
                    .Include(c => c.ServicesRendered)
                    .Select(c => new OrderReadOnlyDto
                        {Id = c.Id, FirstName = c.FirstName, ServicesRenderedCount = c.ServicesRendered.Count})
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                if (retVal.Count == 0) throw new NotFoundException("No Orders found");

                return retVal;
            }
        }
    }
}