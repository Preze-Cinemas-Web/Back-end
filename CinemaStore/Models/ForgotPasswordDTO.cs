using System.ComponentModel.DataAnnotations;

namespace CinemaStore.Models
{
    public class ForgotPasswordDTO
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}