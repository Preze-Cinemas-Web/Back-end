using Cinema.Models;
using CinemaStore.Models;

namespace CinemaStore.Business
{
    public interface IUserService
    {
        public IEnumerable<RegisterUserDTO> FindAllUsers();
        public RegisterUserDTO Register(RegisterUserDTO user);
        public void UpdateVerificationDate(RegisterUserDTO userDTO);
        public string SendEmailVerification(string username, string password);
        public RegisterUserDTO FindUserByEmailVerificationToken(string token);
        public bool isEmailVerified(string token);
        public string Login(LoginUserDTO oldUserDTO);
        public RegisterUserDTO FindUserByUsername(string username);
        public UpdateUserDTO ModifyUser(UpdateUserDTO user);
        public void DeleteUserById(int id);
        public RegisterUserDTO FindUserById(int id);
        public RegisterUserDTO FindUserByEmail(string email);
        public string GenerateAndSetNewPassword(string email);
    }
}