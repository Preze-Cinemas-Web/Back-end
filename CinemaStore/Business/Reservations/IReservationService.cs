using CinemaStore.Models;

namespace CinemaStore.Business.Reservations
{
    public interface IReservationService
    {
        public IEnumerable<ReservationDTO> FindAllReservations();
        Task<bool> MakeReservationAsync(ReservationRequestDTO reserv, int userId);

        public bool ValidateReservation(ConfirmReservationDTO reserv, int userId);

        public string DownloadTickets(int userId);
    }
}
