using CinemaStore.Business.Movies;
using Microsoft.AspNetCore.Mvc;

namespace CinemaStore.Controllers
{
    [Route("API/[controller]")]
    [ApiController]
    public class MoviesController : Controller
    {
        private readonly TMDBService _tmdbService;

        public MoviesController(TMDBService tmdbService)
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


