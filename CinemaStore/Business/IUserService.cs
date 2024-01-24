using Cinema.Models;
using Microsoft.AspNetCore.Mvc;

namespace CinemaStore.Business
{
    public interface IUserService
    {
        // [HttpGet] 
        public IEnumerable<UserDTO> GetAllUsers();
        // [HttpGet("{id:int}")]
        public UserDTO GetUserById(int id);
        // [HttpPost("Login")]
        public bool Login(UserDTO userDTO);
        // [HttpPost("Register")]
        public UserDTO Register(UserDTO user);
        // [HttpPut("{id:int}")]
        public UserDTO UpdateUser(UserDTO user);
        // [HttpDelete("{id:int}")]
        public void DeleteUserById(int id);
    }
}