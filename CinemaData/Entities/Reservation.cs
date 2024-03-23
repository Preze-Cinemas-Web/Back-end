using System.ComponentModel.DataAnnotations;

namespace CinemaData.Entities
{
    public class Reservation
    {
        [Key]
        public int UserId { get; set; }
        [Key]
        public int MovieId { get; set; }
        public int NumberOfTickets { get; set; }
        public int TotalPrice { get; set; } 
    }
}
