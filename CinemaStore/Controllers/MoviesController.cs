using CinemaStore.Business.Movies;
using Microsoft.AspNetCore.Mvc;

namespace CinemaStore.Controllers
{
    [Route("API/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ITMDBService _tmdbService;

        public MoviesController(ITMDBService tmdbService)
        {
            _tmdbService = tmdbService;
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var popularMovies = await _tmdbService.GetPopularMovies();

            return Ok(popularMovies);
        }
    }
}


