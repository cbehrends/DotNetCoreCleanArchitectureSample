namespace Payments.Application.Core.Messaging
{
    public class ClaimPaid 
    {
        public int ClaimId { get; set; }
        public decimal AmountApplied { get; set; }
    }
}