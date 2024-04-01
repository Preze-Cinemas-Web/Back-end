using Cinema.Models;
using CinemaStore.Models;

namespace CinemaStore.Business.Users
{
    public interface IUserService
    {
        public IEnumerable<RegisterUserDTO> FindAllUsers();
        public RegisterUserDTO ModifyUser(RegisterUserDTO user);
        public RegisterUserDTO FindUserById(int id);
        public void DeleteUserById(int id);
    }
}