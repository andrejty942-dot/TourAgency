using System;

namespace TourAgency.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public string ClientFio { get; set; }
        public int TourId { get; set; }
        public Tour TourName { get; set; }
        public int PeopleCount { get; set; }
        public DateTime DepartureDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public Booking()
        {
            ClientFio = string.Empty;
            Status = "Новое";
            Phone = string.Empty;
            Email = string.Empty;
            DepartureDate = DateTime.Now.AddDays(7);
        }
    }
}
