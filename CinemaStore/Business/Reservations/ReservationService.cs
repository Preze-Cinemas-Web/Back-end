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
        public IEnumerable<DownloadTicketsDTO> FindAllReservations()
        {
            var reservsList = _context.Reservation
                .Include(r => r.Movie)
                    .ThenInclude(m => m.Hall)
                .Include(r => r.User)
                .ToList();

            string cinemaCenterName = "Preze Cinemas";
            var ticketsList = new List<DownloadTicketsDTO>();

            foreach (var reserv in reservsList)
            {
                var reservationDTO = new DownloadTicketsDTO
                {
                    CinemaCenter = cinemaCenterName,
                    HallName = reserv.Movie?.Hall?.Name ?? "Unknown Hall",
                    MovieTitle = reserv.Movie?.Title ?? "Unknown Movie",
                    DateTime = $"{reserv.Movie?.DateView} {reserv.Movie?.TimeView}",
                    NumberOfTickets = reserv.NumberOfTickets,
                    TotalValue = reserv.TotalPrice,
                    BookingId = reserv.BookingId,
                };

                ticketsList.Add(reservationDTO);
            }

            return ticketsList;
        }

        /*
         *  HTTP GET - Get Reservations by User Id
         */
        public IEnumerable<DownloadTicketsDTO> FindReservationsByUserId(int userId)
        {
            var reservsList = _context.Reservation
                .Include(r => r.Movie)
                    .ThenInclude(m => m.Hall)
                .Include(r => r.User)
                .Where(u => u.UserId == userId)
                .ToList();

            string cinemaCenterName = "Preze Cinemas";
            var ticketsList = new List<DownloadTicketsDTO>();

            foreach (var reserv in reservsList)
            {
                var reservationDTO = new DownloadTicketsDTO
                {
                    CinemaCenter = cinemaCenterName,
                    HallName = reserv.Movie?.Hall?.Name ?? "Unknown Hall",
                    MovieTitle = reserv.Movie?.Title ?? "Unknown Movie",
                    DateTime = $"{reserv.Movie?.DateView} {reserv.Movie?.TimeView}",
                    NumberOfTickets = reserv.NumberOfTickets,
                    TotalValue = reserv.TotalPrice,
                    BookingId = reserv.BookingId,
                };

                ticketsList.Add(reservationDTO);
            }

            return ticketsList;
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
        public string ValidateReservation(ConfirmReservationDTO reserv, int userId)
        {
            var user = _context.User.FirstOrDefault(u => u.Id == userId);
            var reservation = _context.Reservation
                .Where(u => u.UserId == userId)
                .FirstOrDefault(r => r.BookingId == "");

            if (user == null) 
                return "User not found.";
            if (reservation == null)
                return "Reservation not found.";

            // In case movie is sold out before confirmation
            var movie = _context.Movie
                .FirstOrDefault(m => m.Id == reservation.MovieId);
            if (reservation.NumberOfTickets > movie.AvailableSeats)
                return "Not enough available seats.";

            if (user.FirstName != reserv.FirstName)
                return "First name unconfirmed.";
            if (user.LastName != reserv.LastName)
                return "Last name unconfirmed.";
            if (user.Email != reserv.Email)
                return "Email unconfirmed.";
            if (user.PhoneNumber != reserv.Phone)
                return "Phone unconfirmed.";
            if (user.Birthdate != reserv.Birthdate)
                return "Birthdate unconfirmed.";
            if (reservation.TotalPrice != reserv.Price)
                return "Total price unconfirmed."; ;
;
            return "Reservation confirmed.";
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

        public string DownloadTicketsByBookingId(string bookingId, int userId)
        {
            var reservation = _context.Reservation
                .Include(r => r.Movie)
                    .ThenInclude(m => m.Hall)
                .Include(r => r.User)
                .Where(u => u.UserId == userId)
                .FirstOrDefault(r => r.BookingId == bookingId);

            if (reservation == null)
            {
                return "Reservation not found.";
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

             string filePath = "Tickets/Reservation#" + reservation.BookingId + ".txt";
             using (StreamWriter writer = new StreamWriter(filePath))
             {
                 writer.WriteLine($"Cinema Center: {reservationDTO.CinemaCenter}");
                 writer.WriteLine($"Hall Name: {reservationDTO.HallName}");
                 writer.WriteLine($"Movie Title: {reservationDTO.MovieTitle}");
                 writer.WriteLine($"Date and Time: {reservationDTO.DateTime}");
                 writer.WriteLine($"Number of Tickets: {reservationDTO.NumberOfTickets}");
                 writer.WriteLine($"Total Value: {reservationDTO.TotalValue}");
                 writer.WriteLine($"Booking ID: {reservationDTO.BookingId}");
             } 

            return "Your tickets have been downloaded to "; //+ filePath;
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

            // Delete txt files with booking id as title
            foreach (var reserv in reservations)
            {
                string filePath = "Tickets/Reservation#" + reserv.BookingId + ".txt";
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }

        /*
         *  HTTP DELETE - Delete Reservation by User Id and MovieId
         */
        public string DeleteReservationByBookingId(string bookingId)
        {
            var reservation = _context.Reservation.Include(m => m.Movie).FirstOrDefault(b => b.BookingId == bookingId);

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

            // Delete txt files with booking id as title
            string filePath = "Tickets/Reservation#" + reservation.BookingId + ".txt";
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            return $"Reservation cancelled.\n" + reservationDTO;
        }

    }

}
