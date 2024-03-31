using AutoMapper;
using CinemaData;
using CinemaData.Entities;
using CinemaStore.Business.Users;
using CinemaStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using CinemaStore.Business.Movies;
using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;

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

            var reservationDTO = new ReservationDTO
            {
                UserId = userId,
                MovieId = movie.Id,
                NumberOfTickets = reserv.NumberOfTickets,
                TotalPrice = reserv.NumberOfTickets * 8
            };

            var reservation = this._mapper.Map<Reservation>(reservationDTO);

            await _context.Reservation.AddAsync(reservation);
            movie.AvailableSeats -= reserv.NumberOfTickets;

            await _context.SaveChangesAsync();

            return true;
        }

        public bool ValidateReservation(ConfirmReservationDTO reserv, int userId)
        {
            var user = _context.User.FirstOrDefault(u => u.Id == userId);

            if (user == null) 
                return false;
            if (user.FirstName != reserv.FirstName)
                return false;
            if (user.LastName != reserv.LastName)
                return false;
            if (user.Email != reserv.Email)
                return false;
            if (user.PhoneNumber != reserv.Phone)
                return false;
            if (user.Birthdate != reserv.Birthdate)
                return false;

            return true;
        }
        
        public IEnumerable<DownloadTicketsDTO> DownloadTickets(int userId)
        {
            var reservationsList = _context.Reservation.Include(m => m.Movie).Include(u => u.User).ToList();

            if (reservationsList == null)
                return null;

            var reservationsDTO = reservationsList.Select(reserv => new DownloadTicketsDTO
            {
                FirstName = reserv.User.FirstName,
                LastName = reserv.User.LastName,
                Email = reserv.User.Email,
                Phone = reserv.User.PhoneNumber,
                Birthdate = reserv.User.Birthdate,
                MovieTitle = reserv.Movie.Title,
                TimeView = reserv.Movie.TimeView,
                DateView = reserv.Movie.DateView,
                HallName = "Hall " + reserv.Movie.HallId,
                NumberOfTickets = reserv.NumberOfTickets
            }).ToList();

            return reservationsDTO;
        }
    }

}
