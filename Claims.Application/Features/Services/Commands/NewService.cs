using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Claims.Application.Features.Claims.Commands;
using Claims.Application.Features.Claims.Queries;
using Claims.Application.Interfaces;
using Claims.Domain.Entities;
using MediatR;

namespace Claims.Application.Features.Services.Commands
{
    public static class NewService
    {
        public class Command: IRequest<int>
        {
            public string Description { get; set; }
        }
        
        public class Handler: IRequestHandler<Command, int>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            public Handler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context ?? throw new NullReferenceException("NewService Handler requires a non null IApplicationDbContext");
                _mapper = mapper ?? throw new NullReferenceException("NewService Handler requires a non null IMapper");
            }
            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                
                var newService = _mapper.Map<Service>(request);
                _context.Services.Add(newService);
                await _context.SaveChangesAsync(cancellationToken);
                return newService.Id;
            }
        }
    }
}