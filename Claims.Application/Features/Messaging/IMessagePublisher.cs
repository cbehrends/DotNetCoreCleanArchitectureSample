using System.Threading.Tasks;
using Common.Messaging.Payments;

namespace Claims.Application.Features.Messaging
{
    public interface IMessagePublisher
    {
        Task SendClaimPaymentApproved(IClaimPaymentApproved claimPaymentApproved);
    }
}