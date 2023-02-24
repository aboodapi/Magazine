using System;
using System.Collections.Generic;

#nullable disable

namespace store.Models
{
    public partial class Order
    {
        public decimal Id { get; set; }
        public decimal UserId { get; set; }
        public decimal PayId { get; set; }
        public decimal ProductId { get; set; }
        public DateTime? Datefrom { get; set; }
        public DateTime? Dateto { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Status { get; set; }

        public virtual Payment Pay { get; set; }
        public virtual Product Product { get; set; }
        public virtual User User { get; set; }
    }
}
