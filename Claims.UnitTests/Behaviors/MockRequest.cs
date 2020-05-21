using MediatR;

namespace Claims.UnitTests.Behaviors
{
    public class MockRequest: IRequest
    {
        public string Name { get; set; }
    }
}