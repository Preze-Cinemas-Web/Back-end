using Cinema.Models;
using CinemaStore.Models;

namespace CinemaStore.Business.Authentication
{
    public interface IAuthenticationService
    {
        public RegisterUserDTO Register(RegisterUserDTO user);
        public void HashPassword(string password, ref string hashedPassword);
        public void UpdateVerificationDate(RegisterUserDTO userDTO);
        public string SendEmailVerification(string username, string password);
        public RegisterUserDTO FindUserByEmailVerificationToken(string token);
        public bool isEmailVerified(string token);
        public string Login(LoginUserDTO loginUserDTO);
        public string LoginWithAnswer(ForgotPWUserDTO forgotPWUserDTO);
        public RegisterUserDTO FindUserByUsername(string username);
    }
}
