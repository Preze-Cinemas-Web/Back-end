using System.ComponentModel.DataAnnotations;

namespace CinemaStore.Models
{
    public class MovieDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
    }
}
