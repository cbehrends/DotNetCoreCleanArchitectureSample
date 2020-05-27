using System;
using System.Threading.Tasks;
using Claims.Infrastructure.Messaging;
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
        public async Task<IActionResult> Post()
        {
            await _publisher.SendClaimPaymentApproved(1, "me", DateTimeOffset.Now);
            //await Task.FromResult("");
        
            return Accepted("Message has been sent!");
        }
    }
}