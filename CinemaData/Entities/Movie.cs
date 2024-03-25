using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaData.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string TimeView { get; set; }
        public string DateView { get; set; }
        [ForeignKey("Hall")]
        public int HallId { get; set; }
        public Hall Hall { get; set; }
        public int AvailableSeats { get; set; }
    }
}
