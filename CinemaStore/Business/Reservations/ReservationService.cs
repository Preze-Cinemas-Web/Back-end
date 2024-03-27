using AutoMapper;
using CinemaData;
using CinemaData.Entities;
using CinemaStore.Business.Users;
using CinemaStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

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

        public async Task<bool> MakeReservationAsync(ReservationDTO reserv, int userId)
        {
            const int ticketPrice = 8;

            if (userId == null || movieId == null)
                return false;

         //   int totalPrice = numberOfTickets * ticketPrice;
            
            var reservation = new Reservation
            {
                UserId = userId,
                MovieId = movieId,
          //      NumberOfTickets = numberOfTickets,
           //     TotalPrice = totalPrice
            };

            _context.Reservation.Add(reservation);

            _context.SaveChangesAsync();

            return true;
        }
    }

}
