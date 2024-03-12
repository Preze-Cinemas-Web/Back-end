using Cinema.Models;
using CinemaStore.Business;
using CinemaStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CinemaStore.Controllers
{
    [Route("API/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public AuthenticationController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        [HttpPost]
        [Route("Register")]
        public ActionResult<RegisterUserDTO> AddUser([FromBody] RegisterUserDTO model)
        {
            try
            {
                var userDTO = _userService.FindUserByUsername(model.Username);

                if (userDTO != null)
                    return BadRequest(model.Username + " is not available");

                var originalPassword = model.Password;
                var user = _userService.Register(model);

                var userStatus = _userService.SendEmailVerification(user.Username, originalPassword);
               
                if (userStatus.Equals("Invalid email verification token"))
                {
                    return BadRequest(userStatus);
                }
                if (userStatus.Equals("Email already verified"))
                {
                    return Ok(userStatus);
                }
                if (userStatus.Equals("Email sent for verification"))
                {
                    return user;
                }

                return BadRequest("Another error occured");
            }
            catch (ArgumentNullException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Status = "Error", Message = ex.Message });
            }
            catch (MyException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse { Status = "Error", Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Status = "Error", Message = ex.Message });
            }
        }

        [HttpPut]
        [Route("Verify")]
        public IActionResult VerifyEmail(string token)
        {
            var user = _userService.FindUserByEmailVerificationToken(token);
            var isVerified = _userService.isEmailVerified(token);
            if (isVerified)
            {
                return Ok("Email already verified");
            }
            _userService.UpdateVerificationDate(user);

            return Ok("Email verified successfully");
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult ValidateUser([FromBody] LoginUserDTO model)
        {
            try
            {
                var userStatus = _userService.Login(model);
                
                if (userStatus.Equals("Ο χρήστης δεν βρέθηκε"))
                {
                    return NotFound(userStatus);
                }
                
                if (userStatus.Equals("Λάθος κωδικός"))
                {
                    return Unauthorized(userStatus);
                }
                
                if (userStatus.Equals("Επιτυχής σύνδεση"))
                {
                    var user = _userService.FindUserByUsername(model.Username);
                    
                    if (user.Id > 1)
                    {
                        var token = GetToken(user.Id, "User"); // Pass user id and role to GetToken method
                        var jwtHandler = new JwtSecurityTokenHandler();
                        var tokenString = jwtHandler.WriteToken(token);

                        return Ok(tokenString); // User Token
                    }

                    return Ok(Setup.token); // Admin Token
                }

                return Unauthorized();
            }
            catch (ArgumentNullException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Status = "Error", Message = ex.Message });
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Status = "Error", Message = ex.Message });
            }
        }

        private JwtSecurityToken GetToken(int userId, string role) // Add userId and role parameters
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

            var token = new JwtSecurityToken(
                               issuer: _configuration["JWT:Issuer"],
                               audience: _configuration["JWT:Audience"],
                               expires: DateTime.Now.AddHours(3),
                               claims: new[]
                               {
                                   new Claim("userId", userId.ToString()), // Add userId claim
                                   new Claim("role", role) // Add role claim
                               },
                               signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                               );

            return token;
        }

    }
}
