using Cinema.Models;

namespace CinemaStore.Business
{
    public interface IUsersService
    {
        // [HttpGet] 
        public IEnumerable<UsersDTO> GetAllUsers();
        // [HttpGet("{id:int}")]
        public UsersDTO GetUserById(int id);
        // [HttpPost]
        public UsersDTO CreateUser(UsersDTO user);
        // [HttpPut("{id:int}")]
        public UsersDTO UpdateUser(UsersDTO user);
        // [HttpPatch("{id:int}")]
        public UsersDTO UpdatePartialUser(UsersDTO user);
        // [HttpDelete("{id:int}")]
        public void DeleteUserById(int id);
    }
}
