namespace Common.Messaging.Payments
{
    public class OrderPaid: IOrderPaid
    {
        public int ClaimId { get; set; }
        public decimal AmountApplied { get; set; }
    }
}