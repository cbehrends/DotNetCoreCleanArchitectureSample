namespace Common.Messaging.Payments
{
    public class ClaimPaid: IClaimPaid
    {
        public int ClaimId { get; set; }
        public decimal AmountApplied { get; set; }
    }
}