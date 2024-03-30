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
        
        public string DownloadTickets(int userId)
        {
            var user = _context.User.FirstOrDefault(u => u.Id == userId);

            var emailMime = new MimeMessage();
            emailMime.From.Add(MailboxAddress.Parse("prezecinems@ethereal.email"));
            emailMime.To.Add(MailboxAddress.Parse(user.Email));
            emailMime.Subject = "Download Tickets";
            emailMime.Body = new TextPart(TextFormat.Plain)
            {
                Text = "You can download your tickets by clicking the link below:" + "https://localhost:7236/API/Reservations/Download-Tickets" + user.EmailVerificationToken
                + "\n\n" + "Preze Cinems Development Team"
            };

            using var smtp = new SmtpClient();

            smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(user.Email, password);
            smtp.Send(emailMime);
            smtp.Disconnect(true);

            return "Email sent for verification";
        }
    }

}
