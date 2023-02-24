using System;
using System.Collections.Generic;

#nullable disable

namespace store.Models
{
    public partial class Bank
    {
        public decimal Id { get; set; }
        public string FullName { get; set; }
        public long? CardNumber { get; set; }
        public DateTime? Expration { get; set; }
        public int? Cvv { get; set; }
    }
}
