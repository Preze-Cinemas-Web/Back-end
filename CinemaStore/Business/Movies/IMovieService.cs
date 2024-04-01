using CinemaStore.Models;

namespace CinemaStore.Business.Movies
{
    public interface IMovieService
    {
        IEnumerable<MovieDTO> FindAllMovies();
        Task<MovieDTO> GetMovieByTitleAsync(string title);
    }
}
