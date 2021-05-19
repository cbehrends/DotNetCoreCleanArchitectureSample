namespace Common.Messaging.Payments
{
    public class IOrderPaid
    {
        private int ClaimId { get; set; }
        private decimal AmountApplied { get; set; }
    }
}