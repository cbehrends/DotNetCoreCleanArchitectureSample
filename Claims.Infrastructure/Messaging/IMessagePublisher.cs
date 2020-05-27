using System;
using System.Threading.Tasks;

namespace Claims.Infrastructure.Messaging
{
    public interface IMessagePublisher
    {
        Task SendClaimPaymentApproved(int claimId, string approvedBy, DateTimeOffset approvedOn);
    }
}