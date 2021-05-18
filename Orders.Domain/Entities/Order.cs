using System.Collections.Generic;

namespace Orders.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal AmountDue { get; set; }
        public List<RenderedService> ServicesRendered { get; set; }
    }
}