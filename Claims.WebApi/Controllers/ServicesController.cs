using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Claims.Application.Core.Exceptions;
using Claims.Application.Features.Services.Commands;
using Claims.Application.Features.Services.Queries;
using Claims.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Claims.WebApi.Controllers
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
        [ProducesResponseType(typeof(List<Service>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get()
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
        [ProducesResponseType(typeof(Service), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(int id)
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(NewService.Command command)
        {
            var newService = await _mediator.Send(command);

            return CreatedAtAction(nameof(Get),new {id = newService.Id},newService);
        }
    }
}