using Cinema.Models;
using CinemaStore.Business;
using CinemaStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaStore.Controllers
{
    [Authorize]
    [Route("API/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;

        public UsersController(IUserService userService, IAuthenticationService authenticationService)
        {
            _userService = userService;
            _authenticationService = authenticationService;
        }

        [HttpGet]
        [Route("Get-All-Users"), Authorize(Roles = "Admin")]
        public IActionResult GetAllUsers()
        {
            var users = _userService.FindAllUsers();
            
            return Ok(users);
        }

        [HttpPut("Update")]
        public ActionResult<UpdateUserDTO> UpdateUser(string username, [FromBody] UpdateUserDTO updatedUserDTO)
        {
            try
            {
                if (username != updatedUserDTO.Username)
                {
                    return BadRequest("Invalid username");
                }

                var existingUser = _authenticationService.FindUserByUsername(username);
                
                if (existingUser == null)
                {
                    return NotFound("User not found");
                }

                var updatedUser = _userService.ModifyUser(updatedUserDTO);
                
                return updatedUser;
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

        [HttpGet]
        [Route("Get-User-by-Id"), Authorize] 
        public ActionResult<RegisterUserDTO> GetUserById(int id)
        {
            var user = _userService.FindUserById(id);
            
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet]
        [Route("Get-User-by-Username"), Authorize]
        public ActionResult<RegisterUserDTO> GetUserByUsername(string username)
        {
            var user = _authenticationService.FindUserByUsername(username);
            
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpDelete("Delete"), Authorize(Roles = "Admin")]
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
