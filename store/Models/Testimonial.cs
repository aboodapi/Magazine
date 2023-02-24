using System;
using System.Collections.Generic;

#nullable disable

namespace store.Models
{
    public partial class Testimonial
    {
        public decimal Id { get; set; }
        public string Message { get; set; }
        public decimal? UsersId { get; set; }
        public decimal? StatusId { get; set; }

        public virtual Status Status { get; set; }
        public virtual User Users { get; set; }
    }
}
