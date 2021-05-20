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
    public static class UpdateOrder
    {
        public record Command : IRequest<Order>
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public List<RenderedServiceDto> ServicesRendered { get; set; }
        }

        public class Handler : IRequestHandler<Command, Order>
        {
            private readonly IApplicationDbContext _context;

            public Handler(IApplicationDbContext context)
            {
                _context = context ?? throw new NullReferenceException(nameof(IApplicationDbContext));
            }

            public async Task<Order> Handle(Command request, CancellationToken cancellationToken)
            {
                var updateClaim = await _context
                    .Orders
                    .Include(claim => claim.ServicesRendered)
                    .SingleOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

                // foreach (var renderedServiceDto in request.ServicesRendered)
                // {
                //     var serviceCost = await _context.Services.SingleOrDefaultAsync(s => 
                //             s.Id == renderedServiceDto.ServiceId, cancellationToken: cancellationToken)
                //         .Select(svc =>svc.Cost);
                //     
                //     updateClaim.ServicesRendered.Add(new RenderedService
                //         {ServiceId = renderedServiceDto.ServiceId, Order = updateClaim, Cost = serviceCost});
                // }

                updateClaim.FirstName = request.FirstName;


                var newServiceList = updateClaim
                    .ServicesRendered
                    .Where(renderedService =>
                        request.ServicesRendered.Any(sr => sr.Id == renderedService.Id)).ToList();


                foreach (var renderedService in request.ServicesRendered.Where(renderedService =>
                    newServiceList.Count == 0 || newServiceList.Any(c => c.Id != renderedService.Id)))
                    newServiceList.Add(
                        new RenderedService {ClaimId = request.Id, ServiceId = renderedService.ServiceId});

                updateClaim.ServicesRendered = newServiceList;

                foreach (var renderedService in updateClaim.ServicesRendered)
                {
                    var serviceCost = await _context.Services.SingleOrDefaultAsync(s =>
                            s.Id == renderedService.ServiceId, cancellationToken)
                        .Select(svc => svc.Cost);

                    renderedService.Cost = serviceCost;
                }

                var oldTotal = updateClaim.TotalAmount;
                var newTotal = newServiceList.Sum(svc => svc.Cost);

                if (oldTotal < newTotal)
                {
                    updateClaim.TotalAmount = newServiceList.Sum(svc => svc.Cost);
                    updateClaim.AmountDue += newTotal - oldTotal;
                }

                updateClaim.TotalAmount = newServiceList.Sum(svc => svc.Cost);

                await _context.SaveChangesAsync(cancellationToken);
                return await _context.Orders
                    .Include(cl => cl.ServicesRendered)
                    .ThenInclude<Order, RenderedService, Service>(sr => sr.Service)
                    .SingleAsync(cl => cl.Id == request.Id, cancellationToken);
            }
        }
    }
}