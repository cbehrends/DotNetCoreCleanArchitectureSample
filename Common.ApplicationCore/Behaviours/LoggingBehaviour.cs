using System.Threading;
using System.Threading.Tasks;
using Common.ApplicationCore.Interfaces;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace Common.ApplicationCore.Behaviours
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
            var userName = _currentUserService.UserId;


            _logger.LogInformation($"Request: {requestName} {userId} {userName} {request}");
        }
    }
}