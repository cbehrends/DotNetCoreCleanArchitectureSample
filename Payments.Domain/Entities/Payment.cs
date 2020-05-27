using System;

namespace Payments.Domain.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public int ClaimId { get; set; }
        public decimal PaymentAmount { get; set; }
        public DateTimeOffset PaymentDate { get; set; }
    }
}