namespace Common.Messaging.Payments
{
    public class IClaimPaid
    {
        int ClaimId { get; set; }
        decimal AmountApplied { get; set; }
    }
}