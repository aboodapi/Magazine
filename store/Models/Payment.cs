using System;
using System.Collections.Generic;

#nullable disable

namespace store.Models
{
    public partial class Payment
    {
        public Payment()
        {
            Orders = new HashSet<Order>();
        }

        public decimal Id { get; set; }
        public string Type { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
