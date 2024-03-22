using CinemaStore.Business.Movies;
using Microsoft.AspNetCore.Mvc;

namespace CinemaStore.Controllers
{
    [Route("API/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly TmdbService _tmdbService;

        public MoviesController(TmdbService tmdbService)
        {
            _tmdbService = tmdbService;
        }

        [HttpGet("Get-Popular-Movies")]
        public async Task<IActionResult> GetPopularMovies()
        {
            var movies = await _tmdbService.GetPopularMoviesAsync();
            return Ok(movies);
        }
    }
}


