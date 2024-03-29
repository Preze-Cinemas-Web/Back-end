using System.ComponentModel.DataAnnotations;

namespace CinemaStore.Models
{
    public class ReservationRequestDTO
    {
        [Required]
        public string MovieTitle { get; set; }
        [Required]
        public int NumberOfTickets { get; set; }
    }
}
