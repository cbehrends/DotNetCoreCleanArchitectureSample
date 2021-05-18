using System.Threading.Tasks;
using Common.Messaging.Payments;

namespace Orders.Application.Features.Messaging
{
    public interface IMessagePublisher
    {
        Task SendClaimPaymentApproved(IOrderPaymentApproved orderPaymentApproved);
    }
}