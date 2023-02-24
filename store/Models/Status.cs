using System;
using System.Collections.Generic;

#nullable disable

namespace store.Models
{
    public partial class Status
    {
        public Status()
        {
            Products = new HashSet<Product>();
            Testimonials = new HashSet<Testimonial>();
        }

        public decimal Id { get; set; }
        public string Status1 { get; set; }

        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Testimonial> Testimonials { get; set; }
    }
}
