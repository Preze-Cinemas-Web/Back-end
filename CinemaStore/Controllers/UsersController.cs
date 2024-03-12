using Cinema.Models;
using CinemaStore.Business;
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
        public IActionResult UpdateUser(int id, [FromBody] RegisterUserDTO UpdateduserDTO)
        {
            try
            {
                if (id != UpdateduserDTO.Id)
                {
                    return BadRequest("Invalid user id");
                }

                var existingUser = _userService.FindUserById(id);
                
                if (existingUser == null)
                {
                    return NotFound("User not found");
                }

                var updatedUser = _userService.UpdateUser(UpdateduserDTO);
                
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
