using System.Threading.Tasks;
using NUnit.Framework;

namespace Orders.IntegrationTests
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