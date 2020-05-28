using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Claims.Application.Core;
using Claims.Application.Features.Claims.Model;
using Claims.Domain.Entities;
using MassTransit.Initializers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Claims.Application.Features.Claims.Commands
{
    public static class NewClaim
    {
        public class Command : IRequest<Claim>
        {
            public string FirstName { get; set; }
            public decimal TotalAmount { get; set; }
            public List<RenderedServiceDto> ServicesRendered { get; set; }
        }

        public class Handler : IRequestHandler<Command, Claim>
        {
            private readonly IApplicationDbContext _context;

            public Handler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context ?? throw new NullReferenceException(
                    "NewClaim Handler requires a non null IApplicationDbContext");
            }

            public async Task<Claim> Handle(Command request, CancellationToken cancellationToken)
            {
                var newClaim = new Claim
                {
                    FirstName = request.FirstName,
                    ServicesRendered = new List<RenderedService>(),
                };

                foreach (var renderedServiceDto in request.ServicesRendered)
                {
                    var serviceCost = await _context.Services.SingleOrDefaultAsync(s => 
                        s.Id == renderedServiceDto.ServiceId, cancellationToken: cancellationToken)
                        .Select(svc =>svc.Cost);
                    
                    newClaim.ServicesRendered.Add(new RenderedService
                        {ServiceId = renderedServiceDto.ServiceId, Claim = newClaim, Cost = serviceCost});
                }

                var newClaimTotal = newClaim.ServicesRendered.Sum(sr => sr.Cost);

                newClaim.TotalAmount = newClaimTotal;
                newClaim.AmountDue = newClaimTotal;

                _context.Claims.Add(newClaim);
                await _context.SaveChangesAsync(cancellationToken);
                return await _context
                    .Claims
                    .Include(c => c.ServicesRendered)
                    .ThenInclude(sr => sr.Service)
                    .SingleAsync(c => c.Id == newClaim.Id, cancellationToken);
            }
        }
    }
}