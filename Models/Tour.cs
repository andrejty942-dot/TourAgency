using System;

namespace TourAgency.Models
{
    public class Tour
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public int Duration { get; set; }
        public decimal Price { get; set; }
        public string Hotel { get; set; }

        public Tour()
        {
            Name = string.Empty;
            Country = string.Empty;
            City = string.Empty;
            Hotel = string.Empty;

        }
    }
}
