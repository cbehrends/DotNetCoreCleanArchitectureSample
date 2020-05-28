using System;

namespace Claims.Application.Core.Messaging
{
    public class ClaimPaymentApproved : IClaimPaymentApproved
    {
        public int ClaimId { get; set; }
        public string ApprovedBy { get; set; }
        public decimal PaymentAmount { get; set; }
        public DateTimeOffset ApprovedOn { get; set; }
    }
}