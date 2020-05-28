using System.Reflection.Emit;

namespace Claims.Domain.Entities
{
    public class RenderedService
    {
        public int Id { get; set; }
        public int ClaimId { get; set; }
        public Claim Claim { get; set; }
        public int ServiceId { get; set; }
        public Service Service { get; set; }
        public decimal Cost { get; set; }
    }
}