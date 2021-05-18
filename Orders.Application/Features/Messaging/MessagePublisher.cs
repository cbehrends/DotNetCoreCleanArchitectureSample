using System.Threading.Tasks;
using Common.Messaging.Payments;
using MassTransit;

namespace Orders.Application.Features.Messaging
{
    public class MessagePublisher : IMessagePublisher
    {        
        private readonly IPublishEndpoint _publishEndpoint;

        public MessagePublisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }
        
        public async Task SendClaimPaymentApproved(IOrderPaymentApproved orderPaymentApproved)
        {
            await _publishEndpoint.Publish(orderPaymentApproved);
            
        }
    }
}