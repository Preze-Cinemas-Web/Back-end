using System.ComponentModel.DataAnnotations;

namespace CinemaStore.Models
{
    public class ReservationDTO
    {
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public int NumberOfTickets { get; set; }
        public int TotalPrice { get; set; }
    }
}
