using AutoMapper;
using CinemaData;
using Cinema.Models;

namespace CinemaStore
{
    public class CinemaStoreProfile : Profile
    {
        public CinemaStoreProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
        }
    }
}