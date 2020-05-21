using System.Collections.Generic;
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
                ServicesRendered = new List<Service>()
            };
            
            command.ServicesRendered.Add(new Service{Description = "FOO"});
            
            var claimId = await SendAsync(command);
            
            var newClaim = await SendAsync(new GetClaim.Query{Id = claimId});
            
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
            
            var claimId = await SendAsync(command);
       
            var claim = await SendAsync(new GetClaim.Query{Id = claimId});

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
        
    }
}