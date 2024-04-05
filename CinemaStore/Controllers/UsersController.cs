using Cinema.Models;
using CinemaStore.Business;
using CinemaStore.Business.Authentication;
using CinemaStore.Business.Reservations;
using CinemaStore.Business.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace CinemaStore.Controllers
{
    [Authorize]
    [Route("API/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IReservationService _reservationService;
        private readonly IAuthenticationService _authenticationService;

        public UsersController(IUserService userService, IAuthenticationService authenticationService, IReservationService reservationService)
        {
            _userService = userService;
            _authenticationService = authenticationService;
            _reservationService = reservationService;
        }

        [HttpGet]
        [Route("Get-All-Users"), Authorize(Roles = "Admin")]
        public IActionResult GetAllUsers()
        {
            var users = _userService.FindAllUsers();
            
            return Ok(users);
        }

        [HttpPut("Update")]
        public ActionResult<RegisterUserDTO> UpdateUser([FromBody] RegisterUserDTO updatedUserDTO)
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (string.IsNullOrEmpty(token))
                    return Unauthorized("Invalid token.");

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                var userId = jwtToken.Payload["userId"].ToString();

                var existingUser = _userService.FindUserById(Int32.Parse(userId));

                if (existingUser == null)
                {
                    return NotFound("User not found");
                }

                var updatedUser = _userService.ModifyUser(updatedUserDTO, Int32.Parse(userId));

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

        [HttpGet]
        [Route("Get-User-by-Id"), Authorize(Roles = "Admin")] 
        public ActionResult<RegisterUserDTO> GetUserById(int id)
        {
            var user = _userService.FindUserById(id);
            
            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(user);
        }

        [HttpGet]
        [Route("Get-User"), Authorize]
        public ActionResult<RegisterUserDTO> GetUser()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
                return Unauthorized("Invalid token.");

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            var userId = jwtToken.Payload["userId"].ToString();

            var user = _userService.FindUserById(Int32.Parse(userId));

            if (user == null)
            {
                return NotFound("User not found.");
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
                return NotFound("User not found.");
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
                    return NotFound("User not found.");
                }

                _userService.DeleteUserById(id);
                _reservationService.DeleteReservationsByUserId(id);

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
