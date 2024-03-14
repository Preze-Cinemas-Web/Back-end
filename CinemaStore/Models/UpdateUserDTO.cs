using System.ComponentModel.DataAnnotations;

namespace CinemaStore.Models
{
    public class UpdateUserDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Birthdate { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        [Compare("Password", ErrorMessage = "Error! Passwords do not match")]
        public string? ConfirmPassword { get; set; }
    }
}
