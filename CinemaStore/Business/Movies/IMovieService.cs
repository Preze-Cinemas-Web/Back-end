using CinemaStore.Models;

namespace CinemaStore.Business.Movies
{
    public interface IMovieService
    {
        public IEnumerable<MovieDTO> FindAllMovies();
    }
}
