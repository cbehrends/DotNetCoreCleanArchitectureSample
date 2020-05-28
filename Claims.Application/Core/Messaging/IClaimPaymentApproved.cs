using System;

namespace Claims.Application.Core.Messaging
{
    public interface IClaimPaymentApproved
    {
        int ClaimId { get; }
        string ApprovedBy { get; set; }
        decimal PaymentAmount { get; set; }
        DateTimeOffset ApprovedOn { get; set; }
    }
}