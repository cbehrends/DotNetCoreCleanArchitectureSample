using System.Threading.Tasks;
using Claims.Application.Core.Exceptions;
using Claims.Application.Features.Services.Commands;
using Claims.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;


namespace Claims.IntegrationTests.Features
{
    using static TestFixture;
    
    public class ServicesTests : TestBase
    {
        
        [Test]
        public void Add_Service_Should_Fail_On_Validation_Error()
        {
            var command = new NewService.Command();

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task Should_Add_Service_When_Valid()
        {

            var command = new NewService.Command
            {
                Description = "Test"
            };

            var serviceId = await SendAsync(command);

            var newService = await FindAsync<Service>(serviceId);

            newService.Should().NotBeNull();
            newService.Description.Should().Be(command.Description);

        }

    }
}