using System.Threading.Tasks;
using Claims.Application.Exceptions;
using Claims.Application.Features.Claims.Commands;
using Claims.Application.Features.Claims.Queries;
using Claims.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace Claims.IntegrationTests.Features
{
    using static TestFixture;
    public class ClaimsTests: TestBase
    {
        [Test]
        public void Add_Claim_Should_Validate_Request()
        {
            var command = new NewClaim.Command();

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }
        
        [Test]
        public async Task Should_Add_Claim_When_Valid()
        {
         
            var command = new NewClaim.Command
            {
                FirstName = "Corey"
            };
            
            // var t = new NewClaim.Command();
            
            
            var claimId = await SendAsync(command);
            
            var newClaim = await FindAsync<Claim>(claimId);
            
            newClaim.FirstName.Should().Be("Corey");
        }
        
    }
}