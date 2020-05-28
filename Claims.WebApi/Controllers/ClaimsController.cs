using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Claims.Application.Features.Claims.Commands;
using Claims.Application.Features.Claims.Model;
using Claims.Application.Features.Claims.Queries;
using Claims.WebApi.ViewModels;
using Common.ApplicationCore.Exceptions;
using MediatR;
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
        public async Task<ActionResult<List<ClaimsReadOnlyDto>>> Get()
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
        public async Task<ActionResult<ClaimViewModel>> GetById([FromRoute] int id)
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
        public async Task<ActionResult<ClaimViewModel>> Post([FromBody] NewClaim.Command newClaimCommand)
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
        public async Task<ActionResult<ClaimViewModel>> Put([FromBody] UpdateClaim.Command newClaimCommand)
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