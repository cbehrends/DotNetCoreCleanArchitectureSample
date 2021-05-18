namespace Orders.Application.Features.Orders.Model
{
    public class OrderReadOnlyDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public int ServicesRenderedCount { get; set; }
    }
}