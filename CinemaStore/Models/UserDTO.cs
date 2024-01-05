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
        [MinLength(3, ErrorMessage = "Λάθος όνομα. [1] Το πλήθος των χαρακτήρων πρέπει να είναι τουλάχιστον 3")]   // [1] στο CinemaStore/Business/UserService.cs/CreateUser()
        [MaxLength(15, ErrorMessage = "Λάθος όνομα. [2] Το πλήθος των χαρακτήρων πρέπει να είναι το πολύ 15")]     // [2] στο CinemaStore/Business/UserService.cs/CreateUser()
        public string FirstName { get; set; }
        [MinLength(3, ErrorMessage = "Λάθος επώνυμο. [1] Το πλήθος των χαρακτήρων πρέπει να είναι τουλάχιστον 3")] // [1] στο CinemaStore/Business/UserService.cs/CreateUser()
        [MaxLength(15, ErrorMessage = "Λάθος επώνυμο. [2] Το πλήθος των χαρακτήρων πρέπει να είναι το πολύ 15")]   // [2] στο CinemaStore/Business/UserService.cs/CreateUser()
        public string LastName { get; set; }
        [MinLength(10, ErrorMessage = "Λάθος email. [1] Το πλήθος των χαρακτήρων πρέπει να είναι τουλάχιστον 10")] // [1] στο CinemaStore/Business/UserService.cs/CreateUser()
        [MaxLength(25, ErrorMessage = "Λάθος email. [2] Το πλήθος των χαρακτήρων πρέπει να είναι το πολύ 25")]     // [2] στο CinemaStore/Business/UserService.cs/CreateUser()
        public string Email { get; set; }
        [MinLength(10, ErrorMessage = "Λάθος αριθμός τηλεφώνου. [1] Το πλήθος των χαρακτήρων πρέπει να είναι τουλάχιστον 10")]   // [1] στο CinemaStore/Business/UserService.cs/CreateUser()
        [MaxLength(10, ErrorMessage = "Λάθος αριθμός τηλεφώνου. [2] Το πλήθος των χαρακτήρων πρέπει να είναι το πολύ 10")]       // [2] στο CinemaStore/Business/UserService.cs/CreateUser()
        public string PhoneNumber { get; set; }
        [MinLength(10, ErrorMessage = "Λάθος ημερομηνία γέννησης. [1] Το πλήθος των χαρακτήρων πρέπει να είναι τουλάχιστον 10")] // [1] στο CinemaStore/Business/UserService.cs/CreateUser()
        [MaxLength(10, ErrorMessage = "Λάθος ημερομηνία γέννησης. [2] Το πλήθος των χαρακτήρων πρέπει να είναι το πολύ 10")]     // [2] στο CinemaStore/Business/UserService.cs/CreateUser()
        public string Birthdate { get; set; }
        [MinLength(8, ErrorMessage = "Λάθος όνομα χρήστη. [1] Το πλήθος των χαρακτήρων πρέπει να είναι τουλάχιστον 8")]      // [1] στο CinemaStore/Business/UserService.cs/CreateUser()
        [MaxLength(15, ErrorMessage = "Λάθος όνομα χρήστη. [2] Το πλήθος των χαρακτήρων πρέπει να είναι το πολύ 15")]        // [2] στο CinemaStore/Business/UserService.cs/CreateUser()
        public string Username { get; set; }
        [MinLength(8, ErrorMessage = "Λάθος κωδικός πρόσβασης. [1] Το πλήθος των χαρακτήρων πρέπει να είναι τουλάχιστον 8")] // [1] στο CinemaStore/Business/UserService.cs/CreateUser()
        [MaxLength(15, ErrorMessage = "Λάθος κωδικός πρόσβασης. [2] Το πλήθος των χαρακτήρων πρέπει να είναι το πολύ 15")]   // [2] στο CinemaStore/Business/UserService.cs/CreateUser()
        public string Password { get; set; }
    }
}