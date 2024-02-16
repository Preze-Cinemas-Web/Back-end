using Cinema.Models;
using CinemaStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace CinemaStore.Business
{
    public interface IUserService
    {
        // [HttpGet] 
        public IEnumerable<UserDTO> FindAllUsers();
        // [HttpGet("{id:int}")]
        public UserDTO FindUserById(int id);
        // [HttpGet("{username:string}")]
        public UserDTO FindUserByUsername(string username);
        // [HttpPost("Login")]
        public bool Login(LoginUserDTO oldUserDTO);
        // [HttpPost("Register")]
        public UserDTO Register(UserDTO user);
        // [HttpPut("{id:int}")]
        public UserDTO UpdateUser(UserDTO user);
        // [HttpDelete("{id:int}")]
        public void DeleteUserById(int id);
    }
}