using AutoMapper;
using CinemaData;
using CinemaStore.Models;
using Microsoft.EntityFrameworkCore;



namespace CinemaStore.Business.Movies
{
    public class MovieService : IMovieService
    {
        private readonly CinemaContext _context;
        private readonly IMapper _mapper;

        public MovieService(CinemaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable<MovieDTO> FindAllMovies()
        {
            var movies = _context.Movie.ToList();
            var moviesDTO = _mapper.Map<IEnumerable<MovieDTO>>(movies);

            return moviesDTO;
        }

        public async Task<MovieAvailabilityDTO> GetMovieByTitleAsync(string title)
        {
            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Title == title);

            if (movie == null)
            {
                return null; 
            }

            var movieDto = new MovieAvailabilityDTO
            {
                Title = movie.Title,
                TimeView = movie.TimeView,
                DateView = movie.DateView,
                HallId = movie.HallId,
                AvailableSeats = movie.AvailableSeats
            };

            return movieDto;
        }
    }
}
