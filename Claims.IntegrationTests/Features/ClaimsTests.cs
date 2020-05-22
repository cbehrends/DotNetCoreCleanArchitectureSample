using System.Collections.Generic;
using System.Threading.Tasks;
using Claims.Application.Core.Exceptions;
using Claims.Application.Features.Claims.Commands;
using Claims.Application.Features.Claims.Model;
using Claims.Application.Features.Claims.Queries;
using Claims.Application.Features.Services.Commands;
using Claims.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace Claims.IntegrationTests.Features
{
    using static TestFixture;
    public class ClaimsTests: TestBase
    {
        [Test]
        public void Add_Claim_Should_Fail_On_Validation_Error()
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
                FirstName = "Corey",
                ServicesRendered = new List<AddRenderedServiceDto>()
            };
            
            var newServiceCmd = new NewService.Command
            {
                Description = "Test"
            };

            var serviceId = await SendAsync(newServiceCmd);

            command.ServicesRendered.Add(new AddRenderedServiceDto{ServiceId = serviceId});
            
            var newClaim = await SendAsync(command);

            newClaim.FirstName.Should().Be("Corey");
            newClaim.ServicesRendered.Should().NotBeNullOrEmpty();
            
        }
        
        [Test]
        public async Task Should_Get_Claim_By_Id()
        {
            
            var command = new NewClaim.Command
            {
                FirstName = "Corey"
            };
            
            var newClaim = await SendAsync(command);
       
            var claim = await SendAsync(new GetClaim.Query{Id = newClaim.Id});

            claim.Should().NotBeNull();
            claim.FirstName.Should().Be("Corey");
        }
        
        [Test]
        public async Task Should_Get_All_Claims()
        {
            
            var command = new NewClaim.Command
            {
                FirstName = "Corey"
            };
            
            //Add 2 claims
            await SendAsync(command);
            await SendAsync(command);
       
            var claims = await SendAsync(new GetClaims.Query());

            claims.Should().NotBeNullOrEmpty();
            claims.Count.Should().Be(2);
        }
        
        [Test]
        public async Task Should_Throw_Not_Found_Exception_When_Claim_Not_Found()
        {
            var query = new GetClaim.Query {Id = -999999};

            FluentActions.Invoking(() =>
                SendAsync(query)).Should().Throw<NotFoundException>();
        }
        
        [Test]
        public async Task Should_Throw_Not_Found_Exception_When_No_Claims_Found()
        {
            var query = new GetClaims.Query();

            FluentActions.Invoking(() =>
                SendAsync(query)).Should().Throw<NotFoundException>();
        }
        
        [Test]
        public async Task Should_Delete_Claim_If_Exists()
        {
            var command = new NewClaim.Command
            {
                FirstName = "Corey"
            };
            
            var newClaim = await SendAsync(command);
            
            var deleteClaim = new DeleteClaim.Command{Id = newClaim.Id};

            await SendAsync(deleteClaim);

            var retVal = await FindAsync<Claim>(newClaim.Id);

            retVal.Should().BeNull();

        }
        
        [Test]
        public async Task Should_Throw_Not_Found_On_Delete_Claim_Does_Not_Exist()
        {
            var deleteClaim = new DeleteClaim.Command{Id = -9999};

            FluentActions.Invoking(() =>
                SendAsync(deleteClaim)).Should().Throw<NotFoundException>();

        }
        
    }
}