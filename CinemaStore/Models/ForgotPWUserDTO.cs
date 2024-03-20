using System.ComponentModel.DataAnnotations;

namespace CinemaStore.Models
{
    public class ForgotPWUserDTO
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? SecurityAnswer { get; set; }
    }
}
