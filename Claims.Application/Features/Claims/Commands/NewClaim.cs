using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Claims.Application.Interfaces;
using Claims.Domain.Entities;
using MediatR;

namespace Claims.Application.Features.Claims.Commands
{
    public static class NewClaim
    {
        public class Command : IRequest<int>
        {
            public string FirstName { get; set; } 
            public List<Service> ServicesRendered { get; set; }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            public Handler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context ?? throw new NullReferenceException("NewClaim Handler requires a non null IApplicationDbContext");
                _mapper = mapper ?? throw new NullReferenceException("NewClaim Handler requires a non null IMapper");
            }
            
            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var newClaim = _mapper.Map<Claim>(request);

                _context.Claims.Add(newClaim);
                await _context.SaveChangesAsync(cancellationToken);
                return newClaim.Id;
            }
        }
    }
}