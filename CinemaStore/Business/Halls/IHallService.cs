using CinemaStore.Models;

namespace CinemaStore.Business.Halls
{
    public interface IHallService
    {
        public IEnumerable<HallDTO> FindAllHalls();
    }
}
