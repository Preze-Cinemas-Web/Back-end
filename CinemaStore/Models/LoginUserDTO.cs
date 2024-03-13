using System.ComponentModel.DataAnnotations;

namespace CinemaStore.Models
{
    public class LoginUserDTO
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
