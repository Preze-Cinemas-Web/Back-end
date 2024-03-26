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

        public async Task<bool> MakeReservationAsync(int numberOfTickets)
        {
            const int ticketPrice = 8;

            int userId = GetCurrentUserId();
            int movieId = GetSelectedMovieId();

            if (userId == null || movieId == null)
                return false;

            int totalPrice = numberOfTickets * ticketPrice;
            
            var reservation = new Reservation
            {
                UserId = userId,
                MovieId = movieId,
                NumberOfTickets = numberOfTickets,
                TotalPrice = totalPrice
            };

            _context.Reservation.Add(reservation);

            _context.SaveChangesAsync();

            return true;
        }
        //Den exw idea an doulevei afto
        public int GetCurrentUserId()
        {
            int? userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserId");
            return userId ?? 0;
        }

        public int GetSelectedMovieId()
        {
            return _httpContextAccessor.HttpContext.Items["SelectedMovieId"] as int? ?? 0;
        }


    }

    }
