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
        // [HttpGet("{username:string, password:string}")]
        public bool Login(UserDTO userDTO);
        // [HttpPost]
        public UserDTO Register(UserDTO user);
        // [HttpPut("{id:int}")]
        public UserDTO UpdateUser(UserDTO user);
        // [HttpDelete("{id:int}")]
        public void DeleteUserById(int id);
    }
}