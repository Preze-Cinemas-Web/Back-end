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
        public async Task<string> MakeReservationAsync(ReservationRequestDTO reserv, int userId)
        {
            var hasUnconfirmedReservations = _context.Reservation.
                Where(b => b.BookingId == "").
                Include(m => m.Movie).
                FirstOrDefault(r => r.UserId == userId);


            if (hasUnconfirmedReservations != null)
                return "You have an unconfirmed reservation.\n" +
                    "Movie   : " + hasUnconfirmedReservations.Movie.Title + "\n" +
                    "Tickets : " + hasUnconfirmedReservations.NumberOfTickets + "\n" +
                    "Total   : " + hasUnconfirmedReservations.TotalPrice;

            var movie = _context.Movie.FirstOrDefault(u => u.Title == reserv.MovieTitle);   

            if (movie == null)
                return "Movie not found.";
            if (!(reserv.NumberOfTickets > 0 && reserv.NumberOfTickets <= movie.AvailableSeats)) 
                return "Number of tickets request denied.";
            if (!(reserv.NumberOfTickets <= 9))
                return "You are allowed to book 1-9 tickets.";

            var reservationDTO = new ReservationDTO
            {
                UserId = userId,
                MovieId = movie.Id,
                NumberOfTickets = reserv.NumberOfTickets,
                TotalPrice = reserv.NumberOfTickets * 8,
                BookingId = ""
            };

            var reservation = this._mapper.Map<Reservation>(reservationDTO);

            await _context.Reservation.AddAsync(reservation);

            await _context.SaveChangesAsync();

            return "Reservation request accepted.";
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
;
            return true;
        }

        public string ReturnBookingId(int userId)
        {
            var bookingId = GenerateRandomCode(6);
            while (_context.Reservation.Any(b => b.BookingId == bookingId))
            {
                bookingId = GenerateRandomCode(6);
            }

            var reservation = _context.Reservation.
                Where(b => b.BookingId == "").
                Include(m => m.Movie).
                FirstOrDefault(r => r.UserId == userId);
            
            var movie = _context.Movie.FirstOrDefault(m => m.Id == reservation.MovieId);
            
            movie.AvailableSeats -= reservation.NumberOfTickets;
            reservation.BookingId = bookingId;
            
            _context.SaveChanges();

            return bookingId;
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

        // Delete Reservations by User Id
        public void DeleteReservationsByUserId(int id)
        {
            var reservations = _context.Reservation.Include(m => m.Movie).Where(r => r.UserId == id).ToList();

            if (reservations == null || !reservations.Any())
            {
                return;
            }

            foreach (var reserv in reservations)
            {
                reserv.Movie.AvailableSeats += reserv.NumberOfTickets;
                _context.Reservation.Remove(reserv);
            }

            _context.SaveChanges();
        }

        /*
         *  HTTP DELETE - Delete Reservation by User Id and MovieId
         */
        public string DeleteReservationByUserIdAndMovieId(int userId, int movieId)
        {
            var reservation = _context.Reservation.Include(m => m.Movie).FirstOrDefault(r => r.UserId == userId && r.MovieId == movieId);

            if (reservation == null)
            {
                return "Reservation not found.";
            }
            
            var reservationDTO =  
                    "Movie   : " + reservation.Movie.Title + "\n" +
                    "Tickets : " + reservation.NumberOfTickets + "\n" +
                    "Total   : " + reservation.TotalPrice;

            reservation.Movie.AvailableSeats += reservation.NumberOfTickets;
            _context.Reservation.Remove(reservation);

            _context.SaveChanges();

            return $"Reservation cancelled.\n" + reservationDTO;
        }

    }

}
