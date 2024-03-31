using System.ComponentModel.DataAnnotations;

namespace CinemaStore.Models
{
    public class ConfirmReservationDTO
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Birthdate { get; set; }
    }
}
