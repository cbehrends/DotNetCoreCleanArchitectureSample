namespace Orders.Application.Features.Orders.Model
{
    public class OrderReadOnlyDto
    {
        public int Id { get; init; }
        public string FirstName { get; init; }
        public int ServicesRenderedCount { get; init; }
    }
}