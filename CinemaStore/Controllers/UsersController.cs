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

        public UsersController(IUserService userService)
        {
            _userService = userService;
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

                var existingUser = _userService.FindUserByUsername(username);
                
                if (existingUser == null)
                {
                    return NotFound("User not found");
                }

                var updatedUser = _userService.UpdateUser(updatedUserDTO);
                
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

      /*  [HttpPost("Delete-Request")]
        public IActionResult DeleteRequest(string username)
        {
            
        }  */

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
