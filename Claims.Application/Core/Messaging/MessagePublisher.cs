using System;
using System.Threading.Tasks;
using MassTransit;

namespace Claims.Application.Core.Messaging
{
    public class MessagePublisher : IMessagePublisher
    {        
        private readonly IPublishEndpoint _publishEndpoint;

        public MessagePublisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }
        
        public async Task SendClaimPaymentApproved(IClaimPaymentApproved claimPaymentApproved)
        {
            await _publishEndpoint.Publish(claimPaymentApproved);
            
        }
    }
}