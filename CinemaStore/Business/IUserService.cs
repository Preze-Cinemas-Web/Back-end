using Cinema.Models;
using CinemaStore.Models;

namespace CinemaStore.Business
{
    public interface IUserService
    {
        public IEnumerable<RegisterUserDTO> FindAllUsers();
        public UpdateUserDTO ModifyUser(UpdateUserDTO user);
        public void DeleteUserById(int id);
        public RegisterUserDTO FindUserById(int id);
    }
}