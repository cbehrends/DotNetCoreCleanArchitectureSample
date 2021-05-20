using System;
using System.Threading.Tasks;
using Common.ApplicationCore.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Payments.Application.Features.Payments;
using Payments.Domain.Entities;

namespace Payments.WebApi.Controllers
{
    [ApiController]
    [Route("payments")]
    public class PaymentsController : ControllerBase
    {
        private readonly ILogger<PaymentsController> _logger;
        private readonly IMediator _mediator;

        public PaymentsController(ILogger<PaymentsController> logger, IMediator mediator)
        {
            _logger = logger ?? throw new NullReferenceException(nameof(ILogger<PaymentsController>));
            _mediator = mediator ?? throw new NullReferenceException(nameof(IMediator));
        }

        [HttpGet("/{id:int}")]
        public async Task<ActionResult<Payment>> GetById(int id)
        {
            try
            {
                var payment = await _mediator.Send(new GetPaymentById.Query(id));
                return Ok(payment);
            }
            catch (NotFoundException e)
            {
                _logger.LogInformation(e.Message, e);
                return NotFound(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Payment>> Post(ApplyPayment.Command applyPaymentCommand)
        {
            var newPayment = await _mediator.Send(applyPaymentCommand);

            return CreatedAtAction(nameof(GetById), new {id = newPayment.Id}, newPayment);
        }
    }
}