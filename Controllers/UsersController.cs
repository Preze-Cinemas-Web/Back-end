/*
 *  Οι controller χρησιμοποιούνται για να επικοινωνούν με τα APIs, εδώ θα εκτελέσουμε τις μεθόδους του 
 *  HTTP (GET, POST, PUT, PATCH, DELETE), ώστε να υλοποιήσουμε την μέθοδο CRUD (Create Read Update Delete).
 *  Στα ελληνικά... εδώ θα αλληλεπιδρούμε με την βάση μας:
 *  1) Θα δημιουργούμε μία εγγραφή (Create)
 *  2) Θα διαβάζουμε μία εγγραφή   (Read)
 *  3) Θα ενημερώνουμε μία εγγραφή (Update)
 *  4) θα διαγράφουμε μία εγγραφή  (Delete)
 */
using Cinema.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Controllers
{
    public class UsersController
    {
        [HttpPost]
        public Users CreateUser()
        {
            var user1 = new Users()
            {
                Id = 1,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "prezerak@gmail.com",
                PhoneNumber = "6977777777",
                Birthdate = new DateTime(1970, 01, 27), // new DateTime(YY, MM, DD)
                Username = "gprez",
                Password = "uniwa123"
            };

            return user1;
        }
    }
}
