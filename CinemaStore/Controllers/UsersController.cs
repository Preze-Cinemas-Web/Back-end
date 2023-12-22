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
using CinemaData;
using CinemaStore.Business;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Controllers
{
    [ApiController]
    [Route("/Ath21's API/1.0/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IUsersService _usersService;

        public UsersController(ILogger<UsersController> logger, IUsersService usersService)
        {
            _logger = logger;
            _usersService = usersService;
        }

        [HttpGet]
        public IEnumerable<UsersDTO> GetUsers()
        {
            return _usersService.GetAllUsers();
        }

        [HttpGet("{id:int}")]
        public ActionResult<UsersDTO> GetOneUser(int id) 
        {
            var user = _usersService.GetUserById(id);

            if (user != null)
            {
                return user; 
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public ActionResult<UsersDTO> CreateOneUser(UsersDTO user) 
        {
            try
            {
                return _usersService.CreateUser(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
