using System;

namespace Claims.Application.Core.Messaging
{
    public interface IClaimPaymentApproved
    {
        int ClaimId { get; }
        decimal PaymentAmount { get; }
    }
}