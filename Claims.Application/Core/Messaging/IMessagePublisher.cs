using System;
using System.Threading.Tasks;
using Common.Messaging;
using Common.Messaging.Payments;

namespace Claims.Application.Core.Messaging
{
    public interface IMessagePublisher
    {
        Task SendClaimPaymentApproved(IClaimPaymentApproved claimPaymentApproved);
    }
}