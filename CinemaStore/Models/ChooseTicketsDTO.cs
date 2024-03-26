using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaStore.Models
{
    public class ChooseTicketsDTO
    {
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public int NumberOfTickets { get; set; }
        public int TotalPrice { get; set; }
    }

}

