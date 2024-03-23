using CinemaStore.Business.Halls;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaStore.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("API/[controller]")]
    [ApiController]
    public class HallsController : ControllerBase
    {
        private readonly IHallService _hallService;

        public HallsController(IHallService hallService)
        {
            _hallService = hallService;
        }

        [HttpGet]
        [Route("Get-All-Halls")]
        public IActionResult GetAllHalls()
        {
            var halls = _hallService.FindAllHalls();
            
            return Ok(halls);
        }

    }
}
