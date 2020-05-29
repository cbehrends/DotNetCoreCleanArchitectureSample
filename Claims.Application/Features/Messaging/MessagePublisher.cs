using System.Threading.Tasks;
using Common.Messaging.Payments;
using MassTransit;

namespace Claims.Application.Features.Messaging
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