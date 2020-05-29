using System;
using Common.Messaging;
using Common.Messaging.Payments;

namespace Claims.Application.Core.Messaging
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