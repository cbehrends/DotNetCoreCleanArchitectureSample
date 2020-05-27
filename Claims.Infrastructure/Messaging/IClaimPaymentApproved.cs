using System;

namespace Claims.Infrastructure.Messaging
{
    public interface IClaimPaymentApproved
    {
        int ClaimId { get; }
        string ApprovedBy { get; set; }
        DateTimeOffset ApprovedOn { get; set; }
    }
}