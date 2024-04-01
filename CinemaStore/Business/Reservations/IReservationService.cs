using CinemaStore.Models;

namespace CinemaStore.Business.Reservations
{
    public interface IReservationService
    {
        public IEnumerable<ReservationDTO> FindAllReservations();
        Task<bool> MakeReservationAsync(ReservationRequestDTO reserv, int userId);

        public bool ValidateReservation(ConfirmReservationDTO reserv, int userId);

        public IEnumerable<DownloadTicketsDTO> DownloadTickets(int userId);
        public DownloadTicketsDTO DownloadTicketsByBookingId(string bookingId, int userId);
    }
}
