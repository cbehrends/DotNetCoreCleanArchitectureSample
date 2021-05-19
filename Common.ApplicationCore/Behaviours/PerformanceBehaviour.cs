using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Common.ApplicationCore.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Common.ApplicationCore.Behaviours
{
    public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly ILogger<TRequest> _logger;
        private readonly Stopwatch _timer;

        public PerformanceBehaviour(
            ILogger<TRequest> logger,
            ICurrentUserAccessor currentUserAccessor)
        {
            _timer = new Stopwatch();

            _logger = logger;
            _currentUserAccessor = currentUserAccessor;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            _timer.Start();

            var response = await next();

            _timer.Stop();

            var elapsedMilliseconds = _timer.ElapsedMilliseconds;

            if (elapsedMilliseconds <= 500) return response;

            var requestName = typeof(TRequest).Name;
            var userId = _currentUserAccessor.UserId ?? string.Empty;
            var userName = _currentUserAccessor.UserId;

            _logger.LogWarning(
                $"Long Running Request: {requestName} ({elapsedMilliseconds} milliseconds) {userId} {userName} {request}");

            return response;
        }
    }
}