using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Claims.Application.Exceptions;
using Claims.Application.Features.Claims.Queries;
using Claims.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Claims.WebApi.Controllers
{
    [ApiController]
    [Route("claims")]
    public class ClaimsController: ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ClaimsController> _logger;
        public ClaimsController(IMediator mediator, ILogger<ClaimsController> logger)
        {   
            _mediator = mediator ?? throw new NullReferenceException("Claims Controller requires a non-null IMediator");
            _logger = logger ?? throw new NullReferenceException("Claims Controller requires a non-null ILogger<ClaimsController>");
        }
        
        [HttpGet]
        [ProducesResponseType(type: typeof(List<Claim>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAll()
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
    }
}