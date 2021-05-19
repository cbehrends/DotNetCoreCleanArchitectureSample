using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Common.UnitTests.Behaviors
{
    public class MockUnValidatedRequest : IRequest
    {
    }

    public class MockUnValidatedRequestHandler : IRequestHandler<MockUnValidatedRequest, Unit>
    {
        public Task<Unit> Handle(MockUnValidatedRequest request, CancellationToken cancellationToken)
        {
            return Unit.Task;
        }
    }
}