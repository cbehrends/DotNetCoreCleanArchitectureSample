namespace Common.Messaging.Payments
{
    public class IOrderPaid
    {
        int ClaimId { get; set; }
        decimal AmountApplied { get; set; }
    }
}