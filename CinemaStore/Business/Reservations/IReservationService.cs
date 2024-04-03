using CinemaStore.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaStore.Business.Reservations
{
    public interface IReservationService
    {
        public IEnumerable<DownloadTicketsDTO> FindAllReservations();
        public IEnumerable<DownloadTicketsDTO> FindReservationsByUserId(int userId);
        Task<string> MakeReservationAsync(ReservationRequestDTO reserv, int userId);
        public bool ValidateReservation(ConfirmReservationDTO reserv, int userId);
        public string ReturnBookingId(int userId);
        public DownloadTicketsDTO DownloadTicketsByBookingId(string bookingId, int userId);
        public void DeleteReservationsByUserId(int id);
        public string DeleteReservationByBookingId(string bookingId);
    }
}
