using System;

namespace Claims.Infrastructure.Messaging // Note the namespace must match the namespace of the source message
{
    public interface IClaimPaymentApproved
    {
        int ClaimId { get; }
        string ApprovedBy { get; set; }
        DateTimeOffset ApprovedOn { get; set; }
    }
}