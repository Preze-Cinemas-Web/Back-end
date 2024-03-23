using CinemaStore.Business.Movies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaStore.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("API/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly TmdbService _tmdbService;
        private readonly IMovieService _movieService;

        public MoviesController(TmdbService tmdbService, IMovieService movieService)
        {
            _tmdbService = tmdbService;
            _movieService = movieService;
        }

        [HttpGet]
        [Route("Get-Movies-From-TMDB")]
        public async Task<IActionResult> GetMoviesFromTMDB()
        {
            var movies = await _tmdbService.GetPopularMoviesAsync();
            
            return Ok(movies);
        }

        [HttpGet]
        [Route("Get-Movies-From-PrezeCinemsDB"), Authorize(Roles = "Admin")]
        public IActionResult GetMoviesFromPrezeCinemsDB()
        {
            var movies = _movieService.FindAllMovies();

            return Ok(movies);
        }
    }
}


