using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Orders.Application.Features.Services.Commands;
using Orders.Application.Features.Services.Queries;
using Orders.Domain.Entities;
using Common.ApplicationCore.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Orders.WebApi.Controllers
{
    [ApiController]
    [Route("services")]
    public class ServicesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ServicesController> _logger;

        public ServicesController(IMediator mediator, ILogger<ServicesController> logger)
        {
            _mediator = mediator ??
                        throw new NullReferenceException("Services Controller requires a non-null IMediator");
            _logger = logger ?? throw new NullReferenceException(
                "Services Controller requires a non-null ILogger<ServicesController>");
        }

        [HttpGet]
        public async Task<ActionResult<List<Service>>> Get()
        {
            try
            {
                return Ok(await _mediator.Send(new GetServices.Query()));
            }
            catch (NotFoundException e)
            {
                _logger.LogError(e.Message, e);
                return NotFound(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Service>> Get([FromRoute] int id)
        {
            try
            {
                return Ok(await _mediator.Send(new GetService.Query {Id = id}));
            }
            catch (NotFoundException e)
            {
                _logger.LogError(e.Message, e);
                return NotFound(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Service>> Post([FromBody] NewService.Command command)
        {
            try
            {
                var newService = await _mediator.Send(command);

                return CreatedAtAction(nameof(Get),new {id = newService.Id},newService);
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
                await _mediator.Send(new DeleteService.Command {Id = id});
            }
            catch (EntityInUseException inUseException)
            {
                return BadRequest(inUseException.Message);
            }
            catch (Exception e)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}