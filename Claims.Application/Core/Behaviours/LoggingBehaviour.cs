using System.Threading;
using System.Threading.Tasks;
using Claims.Application.Core.Interfaces;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace Claims.Application.Core.Behaviours
{
    public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly ILogger<TRequest> _logger;
        private readonly ICurrentUserService _currentUserService;

        public LoggingBehaviour(ILogger<TRequest> logger, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _currentUserService = currentUserService;
        }

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var userId = _currentUserService.UserId ?? string.Empty;
            var userName = "Placeholder Name"; // This would come from your Auth layer which I have not implemented here


            _logger.LogInformation($"Request: {requestName} {userId} {userName} {request}");
        }
    }
}