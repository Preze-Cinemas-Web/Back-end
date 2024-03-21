using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

public class MovieController : Controller
{
    private readonly TMDBService _tmdbService;

    public MovieController(TMDBService tmdbService)
    {
        _tmdbService = tmdbService;
    }

    public async Task<IActionResult> Index()
    {
        var popularMovies = await _tmdbService.GetPopularMovies();
        // Επεξεργασία των popularMovies
        return View();
    }
}

