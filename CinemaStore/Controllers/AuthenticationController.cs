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
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IConfiguration configuration, IAuthenticationService authenticationService)
        {
            _configuration = configuration;
            _authenticationService = authenticationService;
        }

        [HttpPost]
        [Route("Register")]
        public ActionResult<RegisterUserDTO> AddUser([FromBody] RegisterUserDTO model)
        {
            try
            {
                var userDTO = _authenticationService.FindUserByUsername(model.Username);

                if (userDTO != null)
                    return BadRequest(model.Username + " is not available");

                var originalPassword = model.Password;
                var user = _authenticationService.Register(model);

                var userStatus = _authenticationService.SendEmailVerification(user.Username, originalPassword);
               
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
                    return Ok(user);
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

        [HttpGet]
        [Route("Verify-Email")]
        public ActionResult<string> VerifyEmail(string token)
        {
            var user = _authenticationService.FindUserByEmailVerificationToken(token);
            var isVerified = _authenticationService.isEmailVerified(token);
            if (isVerified)
            {
                return Ok("Email already verified");
            }
            _authenticationService.UpdateVerificationDate(user);

            return Ok("Email verified successfully");
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult ValidateUser([FromBody] LoginUserDTO model)
        {
            try
            {
                var userStatus = _authenticationService.Login(model);
                
                if (userStatus.Equals("User not found"))
                {
                    return NotFound(userStatus);
                }
                
                if (userStatus.Equals("Incorrect password"))
                {
                    return Unauthorized(userStatus);
                }
                
                if (userStatus.Equals("Successful login"))
                {
                    var user = _authenticationService.FindUserByUsername(model.Username);
                    
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
