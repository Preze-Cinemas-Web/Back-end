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
using CinemaStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

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

        [HttpGet("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IEnumerable<UserDTO> GetAllUsers()
        {
            return _userService.FindAllUsers();
        }

        [HttpGet("{id:int}", Name = "GetUserById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<UserDTO> GetUserById(int id)
        {
            var user = _userService.FindUserById(id);

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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<UserDTO> AddUser(UserDTO userDTO)
        {
            try
            {
                return CreatedAtRoute("GetUserById", new { id = userDTO.Id }, _userService.Register(userDTO));
            }
            catch (ArgumentNullException ex1)
            {
                return BadRequest(ex1.Message);
            }
            catch (MyException ex2)
            {
                return BadRequest(ex2.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ValidateUser(OldUserDTO oldUserDTO)
        {
            try
            {
                if (_userService.Login(oldUserDTO))
                    return Ok();
                else 
                    return Unauthorized();
            }
            catch (ArgumentNullException ex1)
            {
                return BadRequest(ex1.Message);
            }
            catch (MyException ex2)
            {
                return BadRequest(ex2.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("Update/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateUser(int id, [FromBody] UserDTO userDTO)
        {
            try
            {
                if (id != userDTO.Id)
                {
                    return BadRequest("Error!");
                }

                var existingUser = _userService.FindUserById(id);
                if (existingUser == null)
                {
                    return NotFound();
                }

                var updatedUser = _userService.UpdateUser(userDTO);
                return Ok(updatedUser);
            }
            catch (ArgumentNullException ex1)
            {
                return BadRequest(ex1.Message);
            }
            catch (MyException ex2)
            {
                return BadRequest(ex2.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }



        [HttpDelete("Delete/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                var existingUser = _userService.FindUserById(id);
                if (existingUser == null)
                {
                    return NotFound();
                }

                _userService.DeleteUserById(id);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }



    }
}