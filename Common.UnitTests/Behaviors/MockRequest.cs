using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Common.UnitTests.Behaviors
{
    public class MockRequest : IRequest
    {
        public string Name { get; set; }
    }

    public class MockRequestHandler : IRequestHandler<MockRequest, Unit>
    {
        public Task<Unit> Handle(MockRequest request, CancellationToken cancellationToken)
        {
            return Unit.Task;
        }
    }
}