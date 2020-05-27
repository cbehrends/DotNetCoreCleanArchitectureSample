using System;
using System.Threading.Tasks;
using MassTransit;

namespace Claims.Infrastructure.Messaging
{
    public class MessagePublisher : IMessagePublisher
    {        
        private readonly IPublishEndpoint _publishEndpoint;

        public MessagePublisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task SendClaimPaymentApproved(int claimId, string approvedBy, DateTimeOffset approvedOn)
        {
            await _publishEndpoint.Publish<IClaimPaymentApproved>(new { claimId, approvedBy, approvedOn });
        }
    }
}