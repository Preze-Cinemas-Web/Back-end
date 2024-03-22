namespace CinemaStore.Business.Movies
{
    public interface ITMDBService
    {
        public Task<string> GetPopularMovies();
    }
}
