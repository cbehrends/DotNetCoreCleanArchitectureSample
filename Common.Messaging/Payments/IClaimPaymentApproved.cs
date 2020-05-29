namespace Common.Messaging.Payments
{
    public interface IClaimPaymentApproved
    {
        int ClaimId { get; }
        decimal PaymentAmount { get; }
    }
}