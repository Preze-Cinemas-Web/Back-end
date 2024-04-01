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

        /*
         *  HTTP GET - Get All Movies
         */
        public IEnumerable<MovieDTO> FindAllMovies()
        {
            var moviesList = _context.Movie.Include(m => m.Hall).ToList();

            var moviesDTO = moviesList.Select(movie => new MovieDTO
            {
                Id = movie.Id,
                Title = movie.Title,
                TimeView = movie.TimeView,
                DateView = movie.DateView,
                HallName = movie.Hall.Name, // Replace HallId with HallName
                AvailableSeats = movie.AvailableSeats
            }).ToList();

            return moviesDTO;
        }

        /*
         *  HTTP GET - Get Movie by Title
         */
        public async Task<MovieDTO> GetMovieByTitleAsync(string title)
        {
            var movie = await _context.Movie
                .Include(m => m.Hall) // Include Hall entity
                .FirstOrDefaultAsync(m => m.Title == title);

            if (movie == null)
            {
                return null;
            }

            var movieDto = new MovieDTO
            {
                Title = movie.Title,
                TimeView = movie.TimeView,
                DateView = movie.DateView,
                HallName = movie.Hall.Name,
                AvailableSeats = movie.AvailableSeats
            };

            return movieDto;
        }
    }
}
