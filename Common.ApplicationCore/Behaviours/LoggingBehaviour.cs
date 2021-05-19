using System.Threading;
using System.Threading.Tasks;
using Common.ApplicationCore.Interfaces;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace Common.ApplicationCore.Behaviours
{
    public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly ILogger<TRequest> _logger;

        public LoggingBehaviour(ILogger<TRequest> logger, ICurrentUserAccessor currentUserAccessor)
        {
            _logger = logger;
            _currentUserAccessor = currentUserAccessor;
        }

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var userId = _currentUserAccessor.UserId ?? string.Empty;
            var userName = _currentUserAccessor.UserId;


            _logger.LogInformation($"Request: {requestName} {userId} {userName} {request}");
        }
    }
}