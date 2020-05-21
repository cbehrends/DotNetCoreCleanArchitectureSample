using System;
using System.Collections.Generic;

namespace Claims.Domain.Entities
{
    public class Claim
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public List<Service> ServicesRendered { get; set; }
    }
}