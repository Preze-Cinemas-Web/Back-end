/*
 *  Εδώ φτιάχνουμε τις οντότητες που θα ενταχθούν στην βάση δεδομένων 
 *  π.χ Users, Movies κλπ.
 */
namespace Cinema.Models
{
    public class UsersDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Birthdate { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
