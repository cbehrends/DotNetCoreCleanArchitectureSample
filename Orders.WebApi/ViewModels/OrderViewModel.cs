using System.Collections.Generic;

namespace Orders.WebApi.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public decimal AmountDue { get; set; }
        public decimal TotalAmount { get; set; }
        public List<RenderedServiceViewModel> ServicesRendered { get; set; }
    }
}