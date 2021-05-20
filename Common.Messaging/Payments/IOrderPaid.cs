namespace Common.Messaging.Payments
{
    public interface IOrderPaid
    {
        int OrderId { get; set; }
        decimal AmountApplied { get; set; }
    }
}