using System;
using System.Collections.Generic;

#nullable disable

namespace store.Models
{
    public partial class Contact
    {
        public decimal Id { get; set; }
        public string Message { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
