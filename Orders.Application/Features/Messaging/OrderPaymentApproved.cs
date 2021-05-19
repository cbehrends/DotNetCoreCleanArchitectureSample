using Common.Messaging.Payments;

namespace Orders.Application.Features.Messaging
{
    public class OrderPaymentApproved : IOrderPaymentApproved
    {
        public OrderPaymentApproved(int claimId, decimal paymentAmount)
        {
            OrderId = claimId;
            PaymentAmount = paymentAmount;
        }

        public int OrderId { get; }
        public decimal PaymentAmount { get; }
    }
}