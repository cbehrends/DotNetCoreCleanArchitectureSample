using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Claims.Application.Core.Exceptions;
using Claims.Application.Features.Claims.Commands;
using Claims.Application.Features.Claims.Queries;
using Claims.Domain.Entities;
using Claims.WebApi.Controllers;
using Claims.WebApi.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Claims.IntegrationTests.WebApi
{
    public class Claims
    {

        [Test]
        public async Task ClaimsController_Get_Calls_GetClaims_Query_And_Returns_404_If_Not_Found_Result()
        {
            var mockMediator = new Mock<IMediator>();
            var mockLogger = new Mock<ILogger<ClaimsController>>();
            var mockMapper = new Mock<IMapper>();
            mockMediator.Setup(mock => mock.Send(It.IsAny<GetClaims.Query>(), It.IsAny<CancellationToken>()))
                .Throws<NotFoundException>();
            var sut = new ClaimsController(mockMediator.Object, mockLogger.Object, mockMapper.Object);
            var result = await sut.Get();
            
            mockMediator.Verify(mock => mock.Send(It.IsAny<GetClaims.Query>(), It.IsAny<CancellationToken>()));
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            
        }
        
        [Test]
        public async Task ClaimsController_Get_Calls_GetClaims_Query_And_Returns_OkObjectResult()
        {
            var mockMediator = new Mock<IMediator>();
            var mockLogger = new Mock<ILogger<ClaimsController>>();
            var mockMapper = new Mock<IMapper>();
            var claimList = new List<ClaimViewModel> {new ClaimViewModel()};

            mockMapper.Setup(mapper => mapper.Map<List<ClaimViewModel>>(It.IsAny<List<Claim>>())).Returns(claimList);
            
            var sut = new ClaimsController(mockMediator.Object, mockLogger.Object, mockMapper.Object);
            var result = await sut.Get();
            
            mockMediator.Verify(mock => mock.Send(It.IsAny<GetClaims.Query>(), It.IsAny<CancellationToken>()));
            
            Assert.IsInstanceOf<OkObjectResult>(result);
            
        }
        
        [Test]
        public async Task ClaimsController_GetById_Calls_GetById_Query_And_Returns_404_If_Not_Found_Result()
        {
            var mockMediator = new Mock<IMediator>();
            var mockLogger = new Mock<ILogger<ClaimsController>>();
            var mockMapper = new Mock<IMapper>();
            mockMediator.Setup(mock => mock.Send(It.IsAny<GetClaim.Query>(), It.IsAny<CancellationToken>()))
                .Throws<NotFoundException>();
            
            var sut = new ClaimsController(mockMediator.Object, mockLogger.Object, mockMapper.Object);
            var result = await sut.GetById(1);
            mockMediator.Verify(mock => mock.Send(It.IsAny<GetClaim.Query>(), It.IsAny<CancellationToken>()));
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            
        }
        
        [Test]
        public async Task ClaimsController_GetById_Calls_GetById_Query_And_Returns_OkObjectResult()
        {
            var mockMediator = new Mock<IMediator>();
            var mockLogger = new Mock<ILogger<ClaimsController>>();
            var mockMapper = new Mock<IMapper>();
            
            mockMapper.Setup(mapper => mapper.Map<ClaimViewModel>(It.IsAny<Claim>())).Returns(new ClaimViewModel());
            
            var sut = new ClaimsController(mockMediator.Object, mockLogger.Object, mockMapper.Object);
            var result = await sut.GetById(1);
            mockMediator.Verify(mock => mock.Send(It.IsAny<GetClaim.Query>(), It.IsAny<CancellationToken>()));
            Assert.IsInstanceOf<OkObjectResult>(result);
            
        }
        
        [Test]
        public async Task ClaimsController_Post_Calls_CreateClaim_Command_And_Returns_CreatedAtActionResult()
        {
            var mockMediator = new Mock<IMediator>();
            var mockLogger = new Mock<ILogger<ClaimsController>>();
            var mockMapper = new Mock<IMapper>();

            mockMediator.Setup(mock => mock.Send(It.IsAny<NewClaim.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Claim {Id = 1});
            
            mockMapper.Setup(mapper => mapper.Map<ClaimViewModel>(It.IsAny<Claim>())).Returns(new ClaimViewModel());
            
            var sut = new ClaimsController(mockMediator.Object, mockLogger.Object, mockMapper.Object);
            var result = await sut.Post(new NewClaim.Command());
            mockMediator.Verify(mock => mock.Send(It.IsAny<NewClaim.Command>(), It.IsAny<CancellationToken>()));
            Assert.IsInstanceOf<CreatedAtActionResult>(result);
            
        }
        
        [Test]
        public async Task ClaimsController_Delete_Calls_DeleteClaim_Command_And_Returns_NoContent()
        {
            var mockMediator = new Mock<IMediator>();
            var mockLogger = new Mock<ILogger<ClaimsController>>();
            var mockMapper = new Mock<IMapper>();

            mockMediator.Setup(mock => mock.Send(It.IsAny<DeleteClaim.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            var sut = new ClaimsController(mockMediator.Object, mockLogger.Object, mockMapper.Object);
            var result = await sut.Delete(1);
            mockMediator.Verify(mock => mock.Send(It.IsAny<DeleteClaim.Command>(), It.IsAny<CancellationToken>()));
            Assert.IsInstanceOf<NoContentResult>(result);
            
        }
        
        [Test]
        public async Task ClaimsController_Delete_Calls_DeleteClaim_Command_And_Returns_404_If_Not_Found()
        {
            var mockMediator = new Mock<IMediator>();
            var mockLogger = new Mock<ILogger<ClaimsController>>();
            var mockMapper = new Mock<IMapper>();

            mockMediator.Setup(mock => mock.Send(It.IsAny<DeleteClaim.Command>(), It.IsAny<CancellationToken>()))
                .Throws<NotFoundException>();

            var sut = new ClaimsController(mockMediator.Object, mockLogger.Object, mockMapper.Object);
            var result = await sut.Delete(1);
            mockMediator.Verify(mock => mock.Send(It.IsAny<DeleteClaim.Command>(), It.IsAny<CancellationToken>()));
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            
        }
    }
}