using System.ComponentModel.DataAnnotations;

namespace CinemaData
{
    public class User
    {
        public int Id { get; set; }
        [MinLength(3)]
        [MaxLength(15)]
        public string FirstName { get; set; }
        [MinLength(3)]
        [MaxLength(15)]
        public string LastName { get; set; }
        public string Email { get; set; }
        [MaxLength(10)]
        public string PhoneNumber { get; set; }
        public DateTime Birthdate { get; set; }
        [MaxLength(15)]
        public string Username { get; set; }
        [MaxLength(15)]
        public string Password { get; set; }
    }
}
