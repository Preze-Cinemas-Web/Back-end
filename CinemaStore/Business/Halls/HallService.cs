using AutoMapper;
using CinemaData;
using CinemaStore.Models;

namespace CinemaStore.Business.Halls
{
    public class HallService : IHallService
    {
        private readonly CinemaContext _context;
        private readonly IMapper _mapper;

        public HallService(CinemaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

       /*
        *  HTTP GET - Get All Halls
        */
        public IEnumerable<HallDTO> FindAllHalls()
        {
            var halls = _context.Hall.ToList();
            var hallsDTO = _mapper.Map<IEnumerable<HallDTO>>(halls);

            return hallsDTO;
        }
    }
}
