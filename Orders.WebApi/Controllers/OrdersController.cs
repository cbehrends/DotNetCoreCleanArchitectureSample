using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Common.ApplicationCore.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orders.Application.Features.Orders.Commands;
using Orders.Application.Features.Orders.Model;
using Orders.Application.Features.Orders.Queries;
using Orders.WebApi.ViewModels;

namespace Orders.WebApi.Controllers
{
    [ApiController]
    [Route("orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<OrdersController> _logger;
        private readonly IMapper _mapper;

        public OrdersController(IMediator mediator, ILogger<OrdersController> logger, IMapper mapper)
        {
            _mediator = mediator ?? throw new NullReferenceException(nameof(IMediator));
            _logger = logger ?? throw new NullReferenceException(nameof(ILogger<OrdersController>));
            _mapper = mapper ?? throw new NullReferenceException(nameof(IMapper));
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderReadOnlyDto>>> Get()
        {
            try
            {
                return Ok(await _mediator.Send(new GetClaims.Query()));
            }
            catch (NotFoundException e)
            {
                _logger.LogError(e.Message, e);
                return NotFound(e.Message);
            }
        }

        [HttpGet("{id}", Name = "GetById")]
        public async Task<ActionResult<OrderViewModel>> GetById([FromRoute] int id)
        {
            try
            {
                var claim = await _mediator.Send(new GetClaim.Query {Id = id});
                return Ok(_mapper.Map<OrderViewModel>(claim));
            }
            catch (NotFoundException e)
            {
                _logger.LogInformation(e.Message, e);
                return NotFound(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<OrderViewModel>> Post([FromBody] NewOrder.Command newClaimCommand)
        {
            try
            {
                var newClaim = await _mediator.Send(newClaimCommand);

                return CreatedAtAction(nameof(GetById),new {id = newClaim.Id},_mapper.Map<OrderViewModel>(newClaim));
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Errors);
            }
           
        }
        
        [HttpPost("approve_payment/{claimId}")]
        public async Task<IActionResult> ApprovePayment(int claimId)
        {
            await _mediator.Send(new ApprovePayment.Command(claimId));

            return Accepted();

        }
        
        [HttpPut]
        public async Task<ActionResult<OrderViewModel>> Put([FromBody] UpdateOrder.Command newClaimCommand)
        {
            try
            {
                var newClaim = await _mediator.Send(newClaimCommand);

                return Ok(_mapper.Map<OrderViewModel>(newClaim));
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Errors);
            }
           
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                await _mediator.Send(new DeleteOrder.Command {Id = id});
                return NoContent();
            }
            catch (NotFoundException e)
            {
                _logger.LogInformation(e.Message, e);
                return NotFound(e.Message);
            }
        }
    }
}