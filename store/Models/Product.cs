using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace store.Models
{
    public partial class Product
    {
        public Product()
        {
            Orders = new HashSet<Order>();
        }

        public decimal Id { get; set; }
        public string Title { get; set; }
        public string Discription { get; set; }
        public DateTime? PublicationDate { get; set; }
        public decimal? Price { get; set; }
        public string Imagepath { get; set; }
        public decimal? CategoryId { get; set; }
        public decimal? StatusId1 { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }

        public virtual Category Category { get; set; }
        public virtual Status StatusId1Navigation { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
