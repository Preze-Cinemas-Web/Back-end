using AutoMapper;
using CinemaData;
using CinemaData.Entities;
using CinemaStore.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace CinemaStore.Business.Reservations
{
    public class ReservationService : IReservationService
    {
        private readonly CinemaContext _context;
        private readonly IMapper _mapper;
        

        public ReservationService(CinemaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            
        }

        /*
         *  HTTP GET - Get All Reservations
         */
        public IEnumerable<ReservationDTO> FindAllReservations()
        {
            var reservs = _context.Reservation.ToList();
            var reservsDTO = _mapper.Map<IEnumerable<ReservationDTO>>(reservs);

            return reservsDTO;
        }

        /*
         *  HTTP POST - Rervation Request
         */
        public async Task<bool> MakeReservationAsync(ReservationRequestDTO reserv, int userId)
        {
            var movie = _context.Movie.FirstOrDefault(u => u.Title == reserv.MovieTitle);   

            if (userId == null || movie.Id == null)
                return false;
            if (!(reserv.NumberOfTickets > 0 && reserv.NumberOfTickets <= movie.AvailableSeats)) 
                return false;
            if (!(reserv.NumberOfTickets <= 9))
                return false;

            string bookingId = GenerateRandomCode(6);

            var reservationDTO = new ReservationDTO
            {
                UserId = userId,
                MovieId = movie.Id,
                NumberOfTickets = reserv.NumberOfTickets,
                TotalPrice = reserv.NumberOfTickets * 8,
                BookingId = bookingId
            };

            var reservation = this._mapper.Map<Reservation>(reservationDTO);

            await _context.Reservation.AddAsync(reservation);
            movie.AvailableSeats -= reserv.NumberOfTickets;

            await _context.SaveChangesAsync();

            return true;
        }

        /*
         *  HTTP POST - Confirm Reservation
         */
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

        /*
         *  HTTP GET - Download Tickets
         */
        public IEnumerable<DownloadTicketsDTO> DownloadTickets(int userId)
        {
            var reservationsList = _context.Reservation
                .Include(r => r.Movie)
                    .ThenInclude(m => m.Hall)
                .Include(r => r.User)
                .Where(r => r.UserId == userId)
                .ToList();

            if (reservationsList == null || !reservationsList.Any())
            {
                return Enumerable.Empty<DownloadTicketsDTO>();
            }

            string cinemaCenterName = "Preze Cinemas";
            var reservationsDTO = reservationsList.Select(reserv => new DownloadTicketsDTO
            {
                CinemaCenter = cinemaCenterName,
                HallName = reserv.Movie?.Hall?.Name ?? "Unknown Hall",
                MovieTitle = reserv.Movie?.Title ?? "Unknown Movie",
                DateTime = $"{reserv.Movie?.DateView} {reserv.Movie?.TimeView}",
                NumberOfTickets = reserv.NumberOfTickets,
                TotalValue = reserv.TotalPrice,
                BookingId = reserv.BookingId,
            }).ToList();

            return reservationsDTO;
        }

        // Booking Id
        private string GenerateRandomCode(int length)
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

        /*
         *  HTTP GET - Download Tickets by Booking Id
         */
        public DownloadTicketsDTO DownloadTicketsByBookingId(string bookingId, int userId)
        {
            var reservation = _context.Reservation
                .Include(r => r.Movie)
                    .ThenInclude(m => m.Hall)
                .Include(r => r.User)
                .Where(u => u.UserId == userId)
                .FirstOrDefault(r => r.BookingId == bookingId);

            if (reservation == null)
            {
                return null;
            }

            string cinemaCenterName = "Preze Cinemas";
            var reservationDTO = new DownloadTicketsDTO
            {
                CinemaCenter = cinemaCenterName,
                HallName = reservation.Movie?.Hall?.Name ?? "Unknown Hall",
                MovieTitle = reservation.Movie?.Title ?? "Unknown Movie",
                DateTime = $"{reservation.Movie?.DateView} {reservation.Movie?.TimeView}",
                NumberOfTickets = reservation.NumberOfTickets,
                TotalValue = reservation.TotalPrice,
                BookingId = reservation.BookingId,
            };

            return reservationDTO;   
        }

    }

}
