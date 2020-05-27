using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Claims.Application.Core;
using Claims.Domain.Entities;
using Common.ApplicationCore.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Claims.Application.Features.Services.Commands
{
    public static class NewService
    {
        public class Command : IRequest<Service>
        {
            public string Description { get; set; }
        }

        public class Handler : IRequestHandler<Command, Service>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public Handler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context ?? throw new NullReferenceException(
                    "NewService Handler requires a non null IApplicationDbContext");
                _mapper = mapper ?? throw new NullReferenceException("NewService Handler requires a non null IMapper");
            }

            public async Task<Service> Handle(Command request, CancellationToken cancellationToken)
            {
                var newService = _mapper.Map<Service>(request);
                _context.Services.Add(newService);
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                    return newService;
                }
                catch (DbUpdateException)
                {
                    throw new EntityExistsException(newService.Description);
                }
            }
        }
    }
}