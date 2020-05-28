using System;
using System.Threading.Tasks;

namespace Claims.Application.Core.Messaging
{
    public interface IMessagePublisher
    {
        Task SendClaimPaymentApproved(IClaimPaymentApproved claimPaymentApproved);
    }
}