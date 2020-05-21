using System;
using System.Threading;
using System.Threading.Tasks;
using Claims.Application.Features.Claims.Commands;
using MediatR;

namespace Claims.Application.Features.Services.Commands
{
    public static class AddService
    {
        public class Command: IRequest<int>
        {
            public string Description { get; set; }
        }
        
        public class Handler: IRequestHandler<Command, int>
        {
            public Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                return Task.FromResult(-1);
            }
        }
    }
}