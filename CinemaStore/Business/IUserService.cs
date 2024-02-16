using Cinema.Models;
using CinemaStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace CinemaStore.Business
{
    public interface IUserService
    {
        // [HttpGet] 
        public IEnumerable<RegisterUserDTO> FindAllUsers();
        // [HttpGet("{username:string}")]
        public RegisterUserDTO FindUserByUsername(string username);
        // [HttpPost("Login")]
        public bool Login(LoginUserDTO oldUserDTO);
        // [HttpPost("Register")]
        public RegisterUserDTO Register(RegisterUserDTO user);
        // [HttpPut("{id:int}")]
        public RegisterUserDTO UpdateUser(RegisterUserDTO user);
        // [HttpDelete("{id:int}")]
        public void DeleteUserById(int id);
    }
}