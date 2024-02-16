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
        [Route("Get-All-Users")]
        public ActionResult<IEnumerable<RegisterUserDTO>> GetAllUsers()
        {
            var users = _userService.FindAllUsers();
            return Ok(users);
        }

        [HttpGet]
        [Route("Get-User/{username}")]
        public ActionResult<RegisterUserDTO> GetUser(string username)
        {
            var user = _userService.FindUserByUsername(username);
            if (user == null)
                return NotFound();
            return Ok(user);
        }
    }
}
