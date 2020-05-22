using System.Collections.Generic;

namespace Claims.Domain.Entities
{
    public class Claim
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public List<RenderedService> ServicesRendered { get; set; }
    }
}