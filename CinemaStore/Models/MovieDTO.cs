using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaStore.Models
{
    public class MovieDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string TimeView { get; set; }
        public string DateView { get; set; }
        public int HallId { get; set; }
        public int AvailableSeats { get; set; }
    }
}
