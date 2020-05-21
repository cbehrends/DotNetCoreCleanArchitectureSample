using System.Threading.Tasks;
using NUnit.Framework;

namespace Claims.IntegrationTests
{
    using static TestFixture;

    public class TestBase
    {
        [SetUp]
        public async Task TestSetUp()
        {
            await ResetState();
        }
    }
}