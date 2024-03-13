using Cinema.Models;
using CinemaStore.Models;

namespace CinemaStore.Business
{
    public interface IUserService
    {
        // [HttpGet] 
        public IEnumerable<RegisterUserDTO> FindAllUsers();
        // [HttpGet("{id:int}")]
        public RegisterUserDTO FindUserById(int id);
        // [HttpGet("{username:string}")]
        public RegisterUserDTO FindUserByUsername(string username);
        public RegisterUserDTO FindUserByEmailVerificationToken(string token);
        public bool isEmailVerified(string token);
        // [HttpPost("Login")]
        public string Login(LoginUserDTO oldUserDTO);
        // [HttpPost("Register")]
        public RegisterUserDTO Register(RegisterUserDTO user);
        public string SendEmailVerification(string username, string password);
        public void UpdateVerificationDate(RegisterUserDTO userDTO);
        // [HttpPut("{id:int}")]
        public RegisterUserDTO UpdateUser(RegisterUserDTO user);
        // [HttpDelete("{id:int}")]
        public void DeleteUserById(int id);
    }
}