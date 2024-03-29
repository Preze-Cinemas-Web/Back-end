using AutoMapper;
using CinemaData;
using CinemaData.Entities;
using CinemaStore.Business.Users;
using CinemaStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using CinemaStore.Business.Movies;

namespace CinemaStore.Business.Reservations
{
    public class ReservationService : IReservationService
    {
        private readonly CinemaContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReservationService(CinemaContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public IEnumerable<ReservationDTO> FindAllReservations()
        {
            var reservs = _context.Reservation.ToList();
            var reservsDTO = _mapper.Map<IEnumerable<ReservationDTO>>(reservs);

            return reservsDTO;
        }

        public async Task<bool> MakeReservationAsync(ReservationRequestDTO reserv, int userId)
        {
            var movie = _context.Movie.FirstOrDefault(u => u.Title == reserv.MovieTitle);   

            if (userId == null || movie.Id == null)
                return false;
            if (!(reserv.NumberOfTickets > 0 && reserv.NumberOfTickets <= movie.AvailableSeats)) 
                return false;
            if (!(reserv.NumberOfTickets <= 9))
                return false;

            var reservation = new Reservation
            {
                UserId = userId,
                MovieId = movie.Id,
                NumberOfTickets = reserv.NumberOfTickets,
                TotalPrice = reserv.NumberOfTickets * 8
            };

            _context.Reservation.Add(reservation);
            movie.AvailableSeats -= reserv.NumberOfTickets;

            _context.SaveChangesAsync();

            return true;
        }
    }

}
