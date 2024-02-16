using Cinema.Models;
using CinemaStore.Business;
using CinemaStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace CinemaStore.Controllers
{
    [Route("API/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public AuthenticationController( 
            IConfiguration configuration,  
            IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        [HttpPost]
        [Route("Register-User")]
        public ActionResult<RegisterUserDTO> AddUser(RegisterUserDTO model)
        {
            var user = _userService.FindUserByUsername(model.Username);
            if (user != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponseDTO { Status = "Error", Message = "User already exists!" });

            var result = _userService.Register(model);
            if (result != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponseDTO { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            return Ok(new ApiResponseDTO { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<LoginUserDTO>> ValidateUser(LoginUserDTO model)
        {
            var userExists = _userService.FindUserByUsername(model.Username);
            if (userExists != null)
            {
                var matchPassword = _userService.Login(model);
                if (matchPassword)
                {
                    var token = GetToken();

                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    }); 
                }
            }
            return Unauthorized();
        }

        private JwtSecurityToken GetToken()
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

            var token = new JwtSecurityToken(
                               issuer: _configuration["JWT:Issuer"],
                               audience: _configuration["JWT:Audience"],
                               expires: DateTime.Now.AddHours(3),
                               claims: null,
                               signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                               );

            return token;
        }



    }
}
