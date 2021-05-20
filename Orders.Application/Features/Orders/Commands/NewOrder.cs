using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MassTransit.Initializers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Orders.Application.Core;
using Orders.Application.Features.Orders.Model;
using Orders.Domain.Entities;

namespace Orders.Application.Features.Orders.Commands
{
    public static class NewOrder
    {
        public record Command : IRequest<Order>
        {
            public string FirstName { get; set; }
            public decimal TotalAmount { get; set; }
            public List<RenderedServiceDto> ServicesRendered { get; set; }
        }

        public class Handler : IRequestHandler<Command, Order>
        {
            private readonly IApplicationDbContext _context;

            public Handler(IApplicationDbContext context)
            {
                _context = context ?? throw new NullReferenceException(
                    "NewOrder Handler requires a non null IApplicationDbContext");
            }

            public async Task<Order> Handle(Command request, CancellationToken cancellationToken)
            {
                var newClaim = new Order
                {
                    FirstName = request.FirstName,
                    ServicesRendered = new List<RenderedService>()
                };

                foreach (var renderedServiceDto in request.ServicesRendered)
                {
                    var serviceCost = await _context.Services.SingleOrDefaultAsync(s =>
                            s.Id == renderedServiceDto.ServiceId, cancellationToken)
                        .Select(svc => svc.Cost);

                    newClaim.ServicesRendered.Add(new RenderedService
                        {ServiceId = renderedServiceDto.ServiceId, Order = newClaim, Cost = serviceCost});
                }

                var newClaimTotal = newClaim.ServicesRendered.Sum(sr => sr.Cost);

                newClaim.TotalAmount = newClaimTotal;
                newClaim.AmountDue = newClaimTotal;

                _context.Orders.Add(newClaim);
                await _context.SaveChangesAsync(cancellationToken);
                return await _context
                    .Orders
                    .Include(c => c.ServicesRendered)
                    .ThenInclude<Order, RenderedService, Service>(sr => sr.Service)
                    .SingleAsync(c => c.Id == newClaim.Id, cancellationToken);
            }
        }
    }
}