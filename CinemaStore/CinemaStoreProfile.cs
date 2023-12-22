using AutoMapper;
using CinemaData;
using Cinema.Models;

namespace CinemaStore
{
    public class CinemaStoreProfile : Profile
    {
        public CinemaStoreProfile()
        {
            CreateMap<Users, UsersDTO>().ReverseMap();
        }
    }
}
