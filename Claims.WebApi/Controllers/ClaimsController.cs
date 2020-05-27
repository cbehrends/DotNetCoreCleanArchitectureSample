using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Claims.Application.Features.Claims.Commands;
using Claims.Application.Features.Claims.Model;
using Claims.Application.Features.Claims.Queries;
using Claims.Domain.Entities;
using Claims.WebApi.ViewModels;
using Common.ApplicationCore.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Claims.WebApi.Controllers
{
    [ApiController]
    [Route("claims")]
    public class ClaimsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ClaimsController> _logger;
        private readonly IMapper _mapper;

        public ClaimsController(IMediator mediator, ILogger<ClaimsController> logger, IMapper mapper)
        {
            _mediator = mediator ?? throw new NullReferenceException(nameof(IMediator));
            _logger = logger ?? throw new NullReferenceException(nameof(ILogger<ClaimsController>));
            _mapper = mapper ?? throw new NullReferenceException(nameof(IMapper));
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ClaimsReadOnlyDto>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get()
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
        [ProducesResponseType(typeof(Claim), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var claim = await _mediator.Send(new GetClaim.Query {Id = id});
                return Ok(_mapper.Map<ClaimViewModel>(claim));
            }
            catch (NotFoundException e)
            {
                _logger.LogInformation(e.Message, e);
                return NotFound(e.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] NewClaim.Command newClaimCommand)
        {
            try
            {
                var newClaim = await _mediator.Send(newClaimCommand);

                return CreatedAtAction(nameof(GetById),new {id = newClaim.Id},_mapper.Map<ClaimViewModel>(newClaim));
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Errors);
            }
           
        }
        
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put([FromBody] UpdateClaim.Command newClaimCommand)
        {
            try
            {
                var newClaim = await _mediator.Send(newClaimCommand);

                return Ok(_mapper.Map<ClaimViewModel>(newClaim));
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Errors);
            }
           
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                await _mediator.Send(new DeleteClaim.Command {Id = id});
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