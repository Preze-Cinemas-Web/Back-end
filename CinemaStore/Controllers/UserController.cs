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
    [Route("/API/1.0/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpGet]
        public IEnumerable<UserDTO> GetUsers()
        {
            return _userService.GetAllUsers();
        }

        [HttpGet("{id:int}")]
        public ActionResult<UserDTO> GetOneUser(int id)
        {
            var user = _userService.GetUserById(id);

            if (user != null)
            {
                return user;
            }
            else
            {
                return NotFound();
            }
        }


        [HttpPost("Register")]
        public ActionResult<UserDTO> AddUser(UserDTO userDTO)
        {
            try
            {
                return _userService.Register(userDTO);
            }
            catch (ArgumentNullException ex1)
            {
                return BadRequest(ex1.Message);
            }
            catch (MyException ex2)
            {
                return BadRequest(ex2.Message);
            }
        }

        [HttpPost("Login")]
        public ActionResult<UserDTO> ValidateUser(UserDTO userDTO)
        {
            try
            {
                var isValid = _userService.Login(userDTO);
                if (isValid) 
                    return Ok();
                else
                    return NotFound();
            }
            catch (ArgumentNullException ex1)
            {
                return BadRequest(ex1.Message);
            }
            catch (MyException ex2)
            {
                return BadRequest(ex2.Message);
            }
        }
    }
}