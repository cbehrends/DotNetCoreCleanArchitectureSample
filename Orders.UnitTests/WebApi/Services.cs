using System.Threading;
using System.Threading.Tasks;
using Common.ApplicationCore.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Orders.Application.Features.Services.Commands;
using Orders.Application.Features.Services.Queries;
using Orders.Domain.Entities;
using Orders.WebApi.Controllers;

namespace Orders.UnitTests.WebApi
{
    public class Services
    {
        [Test]
        public async Task ServicesController_Get_Calls_GetClaims_Query_And_Returns_404_If_Not_Found_Result()
        {
            var mockMediator = new Mock<IMediator>();
            var mockLogger = new Mock<ILogger<ServicesController>>();

            mockMediator.Setup(mock => mock.Send(It.IsAny<GetServices.Query>(), It.IsAny<CancellationToken>()))
                .Throws<NotFoundException>();

            var sut = new ServicesController(mockMediator.Object, mockLogger.Object);
            var result = await sut.Get();

            mockMediator.Verify(mock => mock.Send(It.IsAny<GetServices.Query>(), It.IsAny<CancellationToken>()));
            Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
        }

        [Test]
        public async Task ServicesController_Get_Calls_GetClaims_Query_And_Returns_OkObjectResult()
        {
            var mockMediator = new Mock<IMediator>();
            var mockLogger = new Mock<ILogger<ServicesController>>();

            var sut = new ServicesController(mockMediator.Object, mockLogger.Object);
            var result = await sut.Get();

            mockMediator.Verify(mock => mock.Send(It.IsAny<GetServices.Query>(), It.IsAny<CancellationToken>()));

            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public async Task ServicesController_GetById_Calls_GetById_Query_And_Returns_404_If_Not_Found_Result()
        {
            var mockMediator = new Mock<IMediator>();
            var mockLogger = new Mock<ILogger<ServicesController>>();

            mockMediator.Setup(mock => mock.Send(It.IsAny<GetService.Query>(), It.IsAny<CancellationToken>()))
                .Throws<NotFoundException>();

            var sut = new ServicesController(mockMediator.Object, mockLogger.Object);
            var result = await sut.Get(1);
            mockMediator.Verify(mock => mock.Send(It.IsAny<GetService.Query>(), It.IsAny<CancellationToken>()));
            Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
        }

        [Test]
        public async Task ServicesController_GetById_Calls_GetById_Query_And_Returns_OkObjectResult()
        {
            var mockMediator = new Mock<IMediator>();
            var mockLogger = new Mock<ILogger<ServicesController>>();


            var sut = new ServicesController(mockMediator.Object, mockLogger.Object);
            var result = await sut.Get(1);
            mockMediator.Verify(mock => mock.Send(It.IsAny<GetService.Query>(), It.IsAny<CancellationToken>()));
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public async Task ClaimsController_Post_Calls_CreateClaim_Command_And_Returns_CreatedAtActionResult()
        {
            var mockMediator = new Mock<IMediator>();
            var mockLogger = new Mock<ILogger<ServicesController>>();


            mockMediator.Setup(mock => mock.Send(It.IsAny<NewService.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Service {Id = 1});


            var sut = new ServicesController(mockMediator.Object, mockLogger.Object);
            var result = await sut.Post(new NewService.Command());
            mockMediator.Verify(mock => mock.Send(It.IsAny<NewService.Command>(), It.IsAny<CancellationToken>()));
            Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);
        }

        [Test]
        public async Task ServicesController_Delete_Calls_DeleteService_Command_And_Returns_NoContent()
        {
            var mockMediator = new Mock<IMediator>();
            var mockLogger = new Mock<ILogger<ServicesController>>();

            mockMediator.Setup(mock => mock.Send(It.IsAny<DeleteService.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            var sut = new ServicesController(mockMediator.Object, mockLogger.Object);
            var result = await sut.Delete(1);
            mockMediator.Verify(mock => mock.Send(It.IsAny<DeleteService.Command>(), It.IsAny<CancellationToken>()));
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task
            ServicesController_Delete_Calls_DeleteService_Command_And_Returns_BadRequest_On_EntityInUseException()
        {
            var mockMediator = new Mock<IMediator>();
            var mockLogger = new Mock<ILogger<ServicesController>>();

            mockMediator.Setup(mock => mock.Send(It.IsAny<DeleteService.Command>(), It.IsAny<CancellationToken>()))
                .Throws<EntityInUseException>();

            var sut = new ServicesController(mockMediator.Object, mockLogger.Object);
            var result = await sut.Delete(1);
            mockMediator.Verify(mock => mock.Send(It.IsAny<DeleteService.Command>(), It.IsAny<CancellationToken>()));
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }
    }
}