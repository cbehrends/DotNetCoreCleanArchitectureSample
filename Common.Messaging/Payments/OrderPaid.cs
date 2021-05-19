namespace Common.Messaging.Payments
{
    public class OrderPaid : IOrderPaid
    {
        public int OrderId { get; set; }
        public decimal AmountApplied { get; set; }
    }
}