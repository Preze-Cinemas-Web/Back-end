namespace CinemaData.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string EmailVerificationToken { get; set; }
        public string EmailVerifiedAt { get; set; }
        public string PhoneNumber { get; set; }
        public string Birthdate { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}