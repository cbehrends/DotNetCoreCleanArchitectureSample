
using Common.ApplicationCore.Interfaces;

namespace Claims.WebApi.Core
{
    public class MockCurrentUserService: ICurrentUserService
    {
        // Mock up a name, normally this would come from the HTTP context from which Auth mechanism is being used.
        private string _userId = "hank@hill.com";

        public string UserId
        {
            get
            {
                return _userId;
            }
        }
    }
}