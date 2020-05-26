using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Claims.Application.Core.Interfaces;
using Claims.Application.Features.Claims.Model;
using Claims.Application.Features.Claims.Queries;
using Claims.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Claims.Application.Features.Claims.Commands
{
    public static class UpdateClaim
    {
        public class Command: IRequest<Claim>
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public List<AddRenderedServiceDto> ServicesRendered { get; set; }
        }

        public class Handler: IRequestHandler<Command, Claim>
        {
            private readonly IApplicationDbContext _context;
            public Handler(IApplicationDbContext context)
            {
                _context = context ?? throw new NullReferenceException(nameof(IApplicationDbContext));
            }
            public async Task<Claim> Handle(Command request, CancellationToken cancellationToken)
            {
                var updateClaim = await _context.Claims.SingleOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
                updateClaim.FirstName = request.FirstName;
                
                await _context.SaveChangesAsync(cancellationToken);
                return updateClaim;
            }
        }
    }
}