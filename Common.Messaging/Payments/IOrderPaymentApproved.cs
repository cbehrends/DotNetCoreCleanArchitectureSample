namespace Common.Messaging.Payments
{
    public interface IOrderPaymentApproved
    {
        int OrderId { get; }
        decimal PaymentAmount { get; }
    }
}