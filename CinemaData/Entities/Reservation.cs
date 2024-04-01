using System.ComponentModel.DataAnnotations;

namespace CinemaData.Entities
{
    public class Reservation
    {
        public int BookingId
        [Key]
        public int UserId { get; set; }
        public User User { get; set; }
        [Key]
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public int NumberOfTickets { get; set; }
        public int TotalPrice { get; set; } 
    }
}
