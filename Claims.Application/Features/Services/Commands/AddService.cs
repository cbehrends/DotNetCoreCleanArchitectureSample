using System;
using System.Threading;
using System.Threading.Tasks;
using Claims.Application.Features.Claims.Commands;
using MediatR;

namespace Claims.Application.Features.Services.Commands
{
    public static class AddService
    {
        public class Command: IRequest<Guid>
        {
            public string Description { get; set; }
        }
        
        public class Handler: IRequestHandler<AddClaim.Command, Guid>
        {
            public Task<Guid> Handle(AddClaim.Command request, CancellationToken cancellationToken)
            {
                return Task.FromResult(Guid.NewGuid());
            }
        }
    }
}