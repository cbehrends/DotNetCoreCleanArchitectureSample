using System.Collections.Generic;
using System.Threading.Tasks;
using Common.ApplicationCore.Exceptions;
using FluentAssertions;
using NUnit.Framework;
using Orders.Application.Features.Orders.Commands;
using Orders.Application.Features.Orders.Model;
using Orders.Application.Features.Services.Commands;
using Orders.Application.Features.Services.Queries;
using Orders.Domain.Entities;

namespace Orders.IntegrationTests.Features
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

            var service = await SendAsync(command);

            var newService = await FindAsync<Service>(service.Id);

            newService.Should().NotBeNull();
            newService.Description.Should().Be(command.Description);
        }

        [Test]
        public async Task Should_Get_Service_By_Id()
        {
            var command = new NewService.Command
            {
                Description = "test"
            };

            var newService = await SendAsync(command);

            var service = await SendAsync(new GetService.Query {Id = newService.Id});

            service.Should().NotBeNull();
            service.Id.Should().BePositive();
            service.Description.Should().Be("test");
        }

        [Test]
        public async Task Should_Get_All_Services()
        {
            var command = new NewService.Command
            {
                Description = "test"
            };

            var command2 = new NewService.Command
            {
                Description = "test2"
            };

            //Add 2 claims
            await SendAsync(command);
            await SendAsync(command2);

            var services = await SendAsync(new GetServices.Query());

            services.Should().NotBeNullOrEmpty();
            services.Count.Should().Be(2);
        }

        [Test]
        public void Should_Throw_Not_Found_Exception_When_Service_Not_Found()
        {
            var query = new GetService.Query {Id = -999999};

            FluentActions.Invoking(() =>
                SendAsync(query)).Should().Throw<NotFoundException>();
        }

        [Test]
        public void Should_Throw_Not_Found_Exception_When_No_Services_Found()
        {
            var query = new GetServices.Query();

            FluentActions.Invoking(() =>
                SendAsync(query)).Should().Throw<NotFoundException>();
        }

        [Test]
        public async Task Should_Delete_Service_If_Exists_And_Not_Linked_To_Claim()
        {
            var command = new NewService.Command
            {
                Description = "Test"
            };

            var svc = await SendAsync(command);

            var deleteService = new DeleteService.Command {Id = svc.Id};

            await SendAsync(deleteService);

            var retVal = await FindAsync<Order>(svc.Id);

            retVal.Should().BeNull();
        }

        [Test]
        public void Should_Throw_Not_Found_On_Delete_If_Service_Does_Not_Exist()
        {
            var deleteService = new DeleteService.Command {Id = -9999};

            FluentActions.Invoking(() =>
                SendAsync(deleteService)).Should().Throw<NotFoundException>();
        }

        [Test]
        public async Task Should_Throw_EntityInUseException_On_Delete_If_Service_Linked_To_Claim()
        {
            var command = new NewService.Command
            {
                Description = "Test"
            };

            var svc = await SendAsync(command);

            var newClaimCmd = new NewOrder.Command
            {
                FirstName = "Corey",
                ServicesRendered = new List<RenderedServiceDto>()
            };

            newClaimCmd.ServicesRendered.Add(new RenderedServiceDto {ServiceId = svc.Id});

            await SendAsync(newClaimCmd);

            var deleteService = new DeleteService.Command {Id = svc.Id};

            FluentActions.Invoking(() =>
                SendAsync(deleteService)).Should().Throw<EntityInUseException>();
        }
    }
}