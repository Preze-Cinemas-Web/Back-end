using AutoMapper;
using CinemaData;
using Cinema.Models;
using CinemaStore.Models;

namespace CinemaStore
{
    public class CinemaStoreProfile : Profile
    {
        public CinemaStoreProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<OldUser, OldUserDTO>().ReverseMap();
        }
    }
}