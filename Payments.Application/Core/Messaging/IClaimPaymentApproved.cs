using System;

namespace Claims.Application.Core.Messaging // Note the namespace must match the namespace of the source message
{
    public interface IClaimPaymentApproved
    {
        int ClaimId { get; }
        string ApprovedBy { get; set; }
        decimal PaymentAmount { get; set; }
        DateTimeOffset ApprovedOn { get; set; }
    }
}