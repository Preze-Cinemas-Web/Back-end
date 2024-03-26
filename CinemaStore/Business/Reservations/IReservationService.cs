using CinemaStore.Models;

namespace CinemaStore.Business.Reservations
{
    public interface IReservationService
    {
        public IEnumerable<ReservationDTO> FindAllReservations();
        Task<bool> MakeReservationAsync(int numberOfTickets);
    }
}
