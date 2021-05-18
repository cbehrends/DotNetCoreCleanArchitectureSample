using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Orders.WebApi.Controllers;
using Orders.WebApi.ViewModels;
using Common.ApplicationCore.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Orders.Application.Features.Orders.Commands;
using Orders.Application.Features.Orders.Queries;
using Orders.Domain.Entities;

namespace Orders.UnitTests.WebApi
{
    public class Orders
    {

        [Test]
        public async Task ClaimsController_Get_Calls_GetClaims_Query_And_Returns_404_If_Not_Found_Result()
        {
            var mockMediator = new Mock<IMediator>();
            var mockLogger = new Mock<ILogger<OrdersController>>();
            var mockMapper = new Mock<IMapper>();
            mockMediator.Setup(mock => mock.Send(It.IsAny<GetClaims.Query>(), It.IsAny<CancellationToken>()))
                .Throws<NotFoundException>();
            var sut = new OrdersController(mockMediator.Object, mockLogger.Object, mockMapper.Object);
            var result = await sut.Get();
            
            mockMediator.Verify(mock => mock.Send(It.IsAny<GetClaims.Query>(), It.IsAny<CancellationToken>()));
            Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
            
        }
        
        [Test]
        public async Task ClaimsController_Get_Calls_GetClaims_Query_And_Returns_OkObjectResult()
        {
            var mockMediator = new Mock<IMediator>();
            var mockLogger = new Mock<ILogger<OrdersController>>();
            var mockMapper = new Mock<IMapper>();
            var claimList = new List<OrderViewModel> {new OrderViewModel()};

            mockMapper.Setup(mapper => mapper.Map<List<OrderViewModel>>(It.IsAny<List<Order>>())).Returns(claimList);
            
            var sut = new OrdersController(mockMediator.Object, mockLogger.Object, mockMapper.Object);
            var result = await sut.Get();
            
            mockMediator.Verify(mock => mock.Send(It.IsAny<GetClaims.Query>(), It.IsAny<CancellationToken>()));
            
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            
        }
        
        [Test]
        public async Task ClaimsController_GetById_Calls_GetById_Query_And_Returns_404_If_Not_Found_Result()
        {
            var mockMediator = new Mock<IMediator>();
            var mockLogger = new Mock<ILogger<OrdersController>>();
            var mockMapper = new Mock<IMapper>();
            mockMediator.Setup(mock => mock.Send(It.IsAny<GetClaim.Query>(), It.IsAny<CancellationToken>()))
                .Throws<NotFoundException>();
            
            var sut = new OrdersController(mockMediator.Object, mockLogger.Object, mockMapper.Object);
            var result = await sut.GetById(1);
            mockMediator.Verify(mock => mock.Send(It.IsAny<GetClaim.Query>(), It.IsAny<CancellationToken>()));
            Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
            
        }
        
        [Test]
        public async Task ClaimsController_GetById_Calls_GetById_Query_And_Returns_OkObjectResult()
        {
            var mockMediator = new Mock<IMediator>();
            var mockLogger = new Mock<ILogger<OrdersController>>();
            var mockMapper = new Mock<IMapper>();
            
            mockMapper.Setup(mapper => mapper.Map<OrderViewModel>(It.IsAny<Claim>())).Returns(new OrderViewModel());
            
            var sut = new OrdersController(mockMediator.Object, mockLogger.Object, mockMapper.Object);
            var result = await sut.GetById(1);
            mockMediator.Verify(mock => mock.Send(It.IsAny<GetClaim.Query>(), It.IsAny<CancellationToken>()));
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            
        }
        
        [Test]
        public async Task ClaimsController_Post_Calls_CreateClaim_Command_And_Returns_CreatedAtActionResult()
        {
            var mockMediator = new Mock<IMediator>();
            var mockLogger = new Mock<ILogger<OrdersController>>();
            var mockMapper = new Mock<IMapper>();

            mockMediator.Setup(mock => mock.Send(It.IsAny<NewOrder.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Order {Id = 1});
            
            mockMapper.Setup(mapper => mapper.Map<OrderViewModel>(It.IsAny<Claim>())).Returns(new OrderViewModel());
            
            var sut = new OrdersController(mockMediator.Object, mockLogger.Object, mockMapper.Object);
            var result = await sut.Post(new NewOrder.Command());
            mockMediator.Verify(mock => mock.Send(It.IsAny<NewOrder.Command>(), It.IsAny<CancellationToken>()));
            Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);
            
        }
        
        [Test]
        public async Task ClaimsController_Post_Calls_CreateClaim_Command_And_Returns_BadRequest_On_ValidationException()
        {
            var mockMediator = new Mock<IMediator>();
            var mockLogger = new Mock<ILogger<OrdersController>>();
            var mockMapper = new Mock<IMapper>();

            mockMediator.Setup(mock => mock.Send(It.IsAny<NewOrder.Command>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ValidationException());
            
            mockMapper.Setup(mapper => mapper.Map<OrderViewModel>(It.IsAny<Claim>())).Returns(new OrderViewModel());
            
            var sut = new OrdersController(mockMediator.Object, mockLogger.Object, mockMapper.Object);
            var result = await sut.Post(new NewOrder.Command());
            mockMediator.Verify(mock => mock.Send(It.IsAny<NewOrder.Command>(), It.IsAny<CancellationToken>()));
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
            
        }
        
        [Test]
        public async Task ClaimsController_Delete_Calls_DeleteClaim_Command_And_Returns_NoContent()
        {
            var mockMediator = new Mock<IMediator>();
            var mockLogger = new Mock<ILogger<OrdersController>>();
            var mockMapper = new Mock<IMapper>();

            mockMediator.Setup(mock => mock.Send(It.IsAny<DeleteOrder.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            var sut = new OrdersController(mockMediator.Object, mockLogger.Object, mockMapper.Object);
            var result = await sut.Delete(1);
            mockMediator.Verify(mock => mock.Send(It.IsAny<DeleteOrder.Command>(), It.IsAny<CancellationToken>()));
            Assert.IsInstanceOf<NoContentResult>(result);
            
        }
        
        [Test]
        public async Task ClaimsController_Delete_Calls_DeleteClaim_Command_And_Returns_404_If_Not_Found()
        {
            var mockMediator = new Mock<IMediator>();
            var mockLogger = new Mock<ILogger<OrdersController>>();
            var mockMapper = new Mock<IMapper>();

            mockMediator.Setup(mock => mock.Send(It.IsAny<DeleteOrder.Command>(), It.IsAny<CancellationToken>()))
                .Throws<NotFoundException>();

            var sut = new OrdersController(mockMediator.Object, mockLogger.Object, mockMapper.Object);
            var result = await sut.Delete(1);
            mockMediator.Verify(mock => mock.Send(It.IsAny<DeleteOrder.Command>(), It.IsAny<CancellationToken>()));
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            
        }
    }
}