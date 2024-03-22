using Cinema.Models;
using CinemaStore.Models;

namespace CinemaStore.Business.Users
{
    public interface IUserService
    {
        public IEnumerable<RegisterUserDTO> FindAllUsers();
        public RegisterUserDTO ModifyUser(RegisterUserDTO user);
        public void DeleteUserById(int id);
        public RegisterUserDTO FindUserById(int id);
    }
}