using Common.Messaging.Payments;

namespace Claims.Application.Features.Messaging
{
    public class ClaimPaymentApproved : IClaimPaymentApproved
    {
        public int ClaimId { get;}
        public decimal PaymentAmount { get; }

        public ClaimPaymentApproved(int claimId, decimal paymentAmount)
        { 
            ClaimId = claimId;
            PaymentAmount = paymentAmount;
        }
    }
}