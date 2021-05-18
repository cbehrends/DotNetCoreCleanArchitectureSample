namespace Orders.WebApi.ViewModels
{
    public class RenderedServiceViewModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ServiceId { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
    }
}