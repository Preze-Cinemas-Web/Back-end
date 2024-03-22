using System.ComponentModel.DataAnnotations;

namespace CinemaStore.Models
{
    public class MovieDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string ReleaseDate { get; set; }
        [Required]
        public string Overview { get; set; }
        [Required]
        public double Popularity { get; set; }
        [Required]
        public string PosterPath { get; set; }
        [Required]
        public double VoteAverage { get; set; }
        [Required]
        public int VoteCount { get; set; }
    }
}
