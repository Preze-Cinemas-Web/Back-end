using AutoMapper;
using Cinema.Models;
using CinemaStore.Models;
using CinemaData.Entities;

namespace CinemaStore
{
    public class CinemaStoreProfile : Profile
    {
        public CinemaStoreProfile()
        {
            CreateMap<User, RegisterUserDTO>().ReverseMap();
            CreateMap<User, UpdateUserDTO>().ReverseMap();
        }
    }
}