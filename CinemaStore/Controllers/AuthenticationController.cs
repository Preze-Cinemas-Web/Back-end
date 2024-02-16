using AutoMapper;
using Cinema.Models;
using CinemaData;
using CinemaStore.Business;
using CinemaStore.Models;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthenticationController> _logger;
        private readonly CinemaContext _context;
        private readonly IUserService _userService;

        public AuthenticationController(
            UserManager<User> userManager, 
            RoleManager<IdentityRole> roleManager, 
            IConfiguration configuration, 
            IMapper mapper, 
            ILogger<AuthenticationController> logger, 
            CinemaContext context,
            IUserService userService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _mapper = mapper;
            _logger = logger;
            _context = context;
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
        [Route("Register-Admin")]
        public async Task<ActionResult<RegisterUserDTO>> AddAdmin(RegisterUserDTO model)
        {
            var userExists = _userService.FindUserByUsername(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponseDTO { Status = "Error", Message = "User already exists!" });

            var result = _userService.Register(model);
            if (result != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponseDTO { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            var user = _mapper.Map<User>(result);

            if (!await _roleManager.RoleExistsAsync(UserRolesDTO.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRolesDTO.Admin));
            if (!await _roleManager.RoleExistsAsync(UserRolesDTO.User))
                await _roleManager.CreateAsync(new IdentityRole(UserRolesDTO.User));

            if (await _roleManager.RoleExistsAsync(UserRolesDTO.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRolesDTO.Admin);
            }
            if (await _roleManager.RoleExistsAsync(UserRolesDTO.User))
            {
                await _userManager.AddToRoleAsync(user, UserRolesDTO.User);
            }

            return Ok(new ApiResponseDTO { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<LoginUserDTO>> ValidateUser(LoginUserDTO model)
        {
            var userExists = _userService.FindUserByUsername(model.Username);
            if (userExists != null)
            {
                var user = _mapper.Map<User>(userExists);
                if (await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    var userRoles = await _userManager.GetRolesAsync(user);

                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };

                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    var token = GetToken(authClaims);

                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    });
                }   
            }
            return Unauthorized();    
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

            var token = new JwtSecurityToken(
                               issuer: _configuration["JWT:Issuer"],
                               audience: _configuration["JWT:Audience"],
                               expires: DateTime.Now.AddHours(3),
                               claims: authClaims,
                               signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                               );

            return token;
        }



    }
}
