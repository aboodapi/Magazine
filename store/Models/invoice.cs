using store.Controllers;
using System.Globalization;

namespace store.Models
{
    public class invoice
    {
        public Payment payment { get; set; }

        public Category category { get; set; }

        public User user { get; set; }

        public Product product { get; set; }

        public Order order { get; set; }

        public Testimonial testimonial { get; set; }

        public Status status { get; set; }

    }

}
