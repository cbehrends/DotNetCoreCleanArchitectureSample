namespace Orders.Domain.Entities
{
    public class RenderedService
    {
        public int Id { get; init; }
        public int ClaimId { get; init; }
        public Order Order { get; init; }
        public int ServiceId { get; init; }
        public Service Service { get; set; }
        public decimal Cost { get; set; }
    }
}