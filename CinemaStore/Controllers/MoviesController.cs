using CinemaStore.Business.Movies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaStore.Controllers
{
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

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("Get-All-Movies-From-TMDB")]
        public async Task<IActionResult> GetMoviesFromTMDB()
        {
            var movies = await _tmdbService.GetPopularMoviesAsync();
            
            return Ok(movies);
        }

        [HttpGet]
        [Route("Get-All-Movies-Views")]
        public IActionResult GetMoviesFromPrezeCinemsDB()
        {
            var movies = _movieService.FindAllMovies();

            return Ok(movies);
        }

        [HttpGet]
        [Route("Get-Movie-Views-by-Title")]
        public async Task<IActionResult> GetMovieAvailability(string title)
        {
            var movie = await _movieService.GetMovieByTitleAsync(title);

            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }
    }
}


