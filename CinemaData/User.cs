using System.ComponentModel.DataAnnotations;

namespace CinemaData
{
    public class User
    {
        public int Id { get; set; }
        [MinLength(3, ErrorMessage = "Λάθος όνομα.\nΟι χαρακτήρες πρέπει να είναι τουλάχιστον 3!!")]
        [MaxLength(15, ErrorMessage = "Λάθος όνομα.\nΟι χαρακτήρες πρέπει να είναι το πολύ 15!!")]
        public string FirstName { get; set; }
        [MinLength(3, ErrorMessage = "Λάθος επώνυμο.\nΟι χαρακτήρες πρέπει να είναι τουλάχιστον 3!!")]
        [MaxLength(15, ErrorMessage = "Λάθος επώνυμο.\nΟι χαρακτήρες πρέπει να είναι το πολύ 15!!")]
        public string LastName { get; set; }
        [MinLength(10, ErrorMessage = "Λάθος email.\nΟι χαρακτήρες πρέπει να είναι τουλάχιστον 10!!")] // Το μικρότερο email format είναι 10 χαρακτήρες και τελειώνει σε @gmail.com
        [MaxLength(25, ErrorMessage = "Λάθος email.\nΟι χαρακτήρες πρέπει να είναι το πολύ 25!!")]
        public string Email { get; set; }
        [MinLength(10, ErrorMessage = "Λάθος αριθμός τηλεφώνου.\nΟι χαρακτήρες πρέπει να είναι τουλάχιστον 10!!")]
        [MaxLength(10, ErrorMessage = "Λάθος αριθμός τηλεφώνου.\nΟι χαρακτήρες πρέπει να είναι το πολύ 10!!")]
        public string PhoneNumber { get; set; }
        [MinLength(10, ErrorMessage = "Λάθος ημερομηνία γέννησης.\nΟι χαρακτήρες πρέπει να είναι τουλάχιστον 10!!")]
        [MaxLength(10, ErrorMessage = "Λάθος ημερομηνία γέννησης.\nΟι χαρακτήρες πρέπει να είναι το πολύ 10!!")]
        public string Birthdate { get; set; }
        [MinLength(8, ErrorMessage = "Λάθος όνομα χρήστη.\nΟι χαρακτήρες πρέπει να είναι τουλάχιστον 8!!")]
        [MaxLength(15, ErrorMessage = "Λάθος όνομα χρήστη.\nΟι χαρακτήρες πρέπει να είναι το πολύ 15!!")]
        public string Username { get; set; }
        [MinLength(8, ErrorMessage = "Λάθος κωδικός πρόσβασης.\nΟι χαρακτήρες πρέπει να είναι τουλάχιστον 8!!")]
        [MaxLength(15, ErrorMessage = "Λάθος κωδικός πρόσβασης.\nΟι χαρακτήρες πρέπει να είναι το πολύ 15!!")]
        public string Password { get; set; }
    }
}
