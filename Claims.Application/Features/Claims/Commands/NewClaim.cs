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
            public List<AddRenderedServiceDto> ServicesRendered { get; set; }
        }

        public class Handler : IRequestHandler<Command, Claim>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public Handler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context ?? throw new NullReferenceException(
                    "NewClaim Handler requires a non null IApplicationDbContext");
                _mapper = mapper ?? throw new NullReferenceException("NewClaim Handler requires a non null IMapper");
            }

            public async Task<Claim> Handle(Command request, CancellationToken cancellationToken)
            {
                var createdClaim = _mapper.Map<Claim>(request);

                _context.Claims.Add(createdClaim);
                await _context.SaveChangesAsync(cancellationToken);
                return await _context
                    .Claims
                    .Include(c => c.ServicesRendered)
                    .ThenInclude(sr => sr.Service)
                    .SingleAsync(c => c.Id == createdClaim.Id, cancellationToken);
            }
        }
    }
}