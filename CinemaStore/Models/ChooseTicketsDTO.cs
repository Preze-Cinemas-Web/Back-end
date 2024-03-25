using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaStore.Models
{
    public class ChooseTicketsDTO
    {
        public string Title { get; set; }
        public string TimeView { get; set; }
        public string DateView { get; set; }
        public string HallName { get; set; }
        public int AvailableSeats { get; set; }
    }
}

