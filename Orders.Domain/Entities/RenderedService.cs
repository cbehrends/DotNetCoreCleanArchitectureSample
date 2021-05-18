using System.Reflection.Emit;

namespace Orders.Domain.Entities
{
    public class RenderedService
    {
        public int Id { get; set; }
        public int ClaimId { get; set; }
        public Order Order { get; set; }
        public int ServiceId { get; set; }
        public Service Service { get; set; }
        public decimal Cost { get; set; }
    }
}