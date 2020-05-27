using System;
using System.Threading.Tasks;
using Claims.Infrastructure.Messaging;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Payments.Infrastructure.Messaging
{
    public class PaymentApprovedConsumer: IConsumer<IClaimPaymentApproved>
    {
        private readonly ILogger<PaymentApprovedConsumer> _logger;
        
        public PaymentApprovedConsumer(ILogger<PaymentApprovedConsumer> logger)
        {
            _logger = logger ?? throw new NullReferenceException(nameof(ILogger<PaymentApprovedConsumer>));
        }
        public async Task Consume(ConsumeContext<IClaimPaymentApproved> context)
        {
            _logger.LogInformation($"Payment approved for claim {context.Message.ClaimId.ToString()}");

            await context.RespondAsync<IMessageAccepted>(new MessageAccepted {Accepted = true});
        }
        
    }
}