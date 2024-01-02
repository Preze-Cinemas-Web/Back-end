using Cinema.Models;

namespace CinemaStore.Business
{
    public interface IUserService
    {
        // [HttpGet] 
        public IEnumerable<UserDTO> GetAllUsers();
        // [HttpGet("{id:int}")]
        public UserDTO GetUserById(int id);
        // [HttpPost]
        public UserDTO CreateUser(UserDTO user);
        // [HttpPut("{id:int}")]
        public UserDTO UpdateUser(UserDTO user);
        // [HttpPatch("{id:int}")]
        public UserDTO UpdatePartialUser(UserDTO user);
        // [HttpDelete("{id:int}")]
        public void DeleteUserById(int id);
    }
}