using System.Collections.Generic;

namespace Claims.WebApi.ViewModels
{
    public class ClaimViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public decimal AmountDue { get; set; }
        public decimal TotalAmount { get; set; }
        public List<RenderedServiceViewModel> ServicesRendered { get; set; }
    }
}