/*
*  Εδώ φτιάχνουμε τις οντότητες που θα ενταχθούν στην βάση δεδομένων 
*  π.χ User, Movie κλπ.
*/
using System.ComponentModel.DataAnnotations;

namespace Cinema.Models
{
    public class UserDTO
    {
        public int Id { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Birthdate { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}