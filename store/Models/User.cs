using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace store.Models
{
    public partial class User
    {
        public User()
        {
            Orders = new HashSet<Order>();
            Testimonials = new HashSet<Testimonial>();
        }

        public decimal Id { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Imagepath { get; set; }
        public decimal? RoleId { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Testimonial> Testimonials { get; set; }
    }
}
