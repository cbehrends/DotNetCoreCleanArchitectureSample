using Common.ApplicationCore.Interfaces;

namespace Orders.WebApi.Core
{
    public class MockCurrentUserAccessor : ICurrentUserAccessor
    {
        // Mock up a name, normally this would come from the HTTP context from which Auth mechanism is being used.

        public string UserId { get; } = "hank@hill.com";
    }
}