using System;
using System.Threading;
using System.Threading.Tasks;
using Common.ApplicationCore.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Payments.Application.Features.Payments;
using Payments.Domain.Entities;
using Payments.WebApi.Controllers;

namespace Payments.UnitTests.WebApi
{
    public class Payments
    {
        private Mock<ILogger<PaymentsController>> _mockLogger;
        private Mock<IMediator> _mockMediator;

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _mockLogger = new Mock<ILogger<PaymentsController>>();
        }

        [Test]
        public void Controller_Should_Call_Apply_Payment_Command_On_Post()
        {
            var sut = new PaymentsController(_mockLogger.Object, _mockMediator.Object);

            sut.Post(new ApplyPayment.Command
            {
                OrderId = 1,
                PaymentAmount = 200
            });

            _mockMediator.Verify(med => med.Send(It.IsAny<ApplyPayment.Command>(), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task Controller_Should_Call_GetPaymentById_Query_And_Return_OkObjectResult()
        {
            var payment = new Payment
            {
                Id = 1,
                OrderId = 2,
                PaymentAmount = 200,
                PaymentDate = DateTimeOffset.Now
            };

            _mockMediator.Setup(mock =>
                    mock.Send(It.IsAny<GetPaymentById.Query>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(payment);

            var sut = new PaymentsController(_mockLogger.Object, _mockMediator.Object);
            var result = await sut.GetById(1);
            _mockMediator.Verify(mock => mock.Send(It.IsAny<GetPaymentById.Query>(), It.IsAny<CancellationToken>()));
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public async Task Controller_Should_Call_GetPaymentById_Query_And_Return_404_If_Not_Foud()
        {
            _mockMediator.Setup(mock =>
                    mock.Send(It.IsAny<GetPaymentById.Query>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new NotFoundException("Payment not found"));

            var sut = new PaymentsController(_mockLogger.Object, _mockMediator.Object);
            var result = await sut.GetById(1);
            _mockMediator.Verify(mock => mock.Send(It.IsAny<GetPaymentById.Query>(), It.IsAny<CancellationToken>()));
            Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
        }
    }
}