/*
*  Εδώ φτιάχνουμε τις οντότητες που θα ενταχθούν στην βάση δεδομένων 
*  π.χ User, Movie κλπ.
*/
using System.ComponentModel.DataAnnotations;

namespace Cinema.Models
{
    public class RegisterUserDTO
    {
        public int Id { get; set; }
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? PhoneNumber { get; set; }
        [Required]
        public string? Birthdate { get; set; }
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required, Compare("Password", ErrorMessage = "Error! Passwords do not match")]
        public string? ConfirmPassword { get; set; }
        [Required]
        public string? SecurityAnswer { get; set; }
        }
}