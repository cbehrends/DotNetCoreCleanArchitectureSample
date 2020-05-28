using System;

namespace Claims.Application.Core.Messaging // Note the namespace must match the namespace of the source message
{
    public interface IClaimPaymentApproved
    {
        int ClaimId { get; }
        decimal PaymentAmount { get;}
    }
}