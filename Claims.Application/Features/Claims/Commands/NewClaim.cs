using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Claims.Application.Core.Interfaces;
using Claims.Application.Features.Claims.Model;
using Claims.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Claims.Application.Features.Claims.Commands
{
    public static class NewClaim
    {
        public class Command : IRequest<Claim>
        {
            public string FirstName { get; set; }
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
                    ServicesRendered = new List<RenderedService>()
                };

                foreach (var renderedServiceDto in request.ServicesRendered)
                {
                    newClaim.ServicesRendered.Add(new RenderedService{ServiceId = renderedServiceDto.ServiceId, Claim = newClaim});
                }

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