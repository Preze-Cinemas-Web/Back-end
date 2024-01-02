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
using CinemaStore.Business;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Controllers
{
    [ApiController]
    [Route("/Ath21's API/1.0/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IUserService _usersService;

        public UserController(ILogger<UserController> logger, IUserService usersService)
        {
            _logger = logger;
            _usersService = usersService;
        }
    }
}
