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
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using System;
using System.Text;

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
            var reservationsList = _context.Reservation
                .Include(r => r.Movie)
                    .ThenInclude(m => m.Hall)
                .Include(r => r.User)
                .Where(r => r.UserId == userId)
                .ToList();

            if (reservationsList == null)
                return null;

            string cinemaCenterName = "Preze Cinemas";
            var reservationsDTO = new List<DownloadTicketsDTO>();
            foreach (var reservation in reservationsList)
            {
                string bookingId = GenerateRandomCode(6);
                string dateTimeString = $"{reservation.Movie.DateView} {reservation.Movie.TimeView}";

                var reservationDTO = new DownloadTicketsDTO
                {
                    CinemaCenter = cinemaCenterName,
                    HallName = reservation.Movie.Hall.Name,
                    MovieTitle = reservation.Movie.Title,
                    DateTime = dateTimeString,
                    NumberOfTickets = reservation.NumberOfTickets,
                    TotalValue = reservation.TotalPrice,
                    BookingId = bookingId
                };

                reservationsDTO.Add(reservationDTO);
            }

            return reservationsDTO;
        }

        string GenerateRandomCode(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"; // Define characters to use
            var random = new Random();
            var code = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                code.Append(chars[random.Next(chars.Length)]);
            }

            return code.ToString();
        }
    }

}
