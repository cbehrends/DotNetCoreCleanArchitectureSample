using System;
using System.Threading.Tasks;
using Claims.Application.Core.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace Claims.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IMessagePublisher _publisher;

        public MessagesController(IMessagePublisher publisher)
        {
            _publisher = publisher;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ClaimPaymentApproved paymentApproved )
        {
            await _publisher.SendClaimPaymentApproved(paymentApproved);
        
            return Accepted("Message has been sent!");
        }
        
        
    }
}