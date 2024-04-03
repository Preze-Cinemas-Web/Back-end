using AutoMapper;
using Cinema.Models;
using CinemaData;
using CinemaData.Entities;
using CinemaStore.Business.Authentication;

namespace CinemaStore.Business.Users
{
    public class UserService : IUserService
    {
        private CinemaContext _context;
        private IMapper _mapper;
        private IAuthenticationService _authenticationService;

        public UserService(CinemaContext context, IMapper mapper, IAuthenticationService authenticationService)
        {
            _context = context;
            _mapper = mapper;
            _authenticationService = authenticationService;
        }

        /*
         * HTTP GET - Get All Users 
         */
        public IEnumerable<RegisterUserDTO> FindAllUsers()
        {
            return _mapper.Map<IEnumerable<RegisterUserDTO>>(_context.User);
        }

        /*
         * HTTP PUT - Update User
         */
        public RegisterUserDTO ModifyUser(RegisterUserDTO updatedUserDTO, int userId)
        {
            string firstName = updatedUserDTO.FirstName;
            UserBusinessLogic.DefineNameBL(firstName, "First Name");

            string lastName = updatedUserDTO.LastName;
            UserBusinessLogic.DefineNameBL(lastName, "Last Name");

            string email = updatedUserDTO.Email;
            UserBusinessLogic.DefineEmailBL(email);

            string phoneNumber = updatedUserDTO.PhoneNumber;
            UserBusinessLogic.DefinePhoneNumberBL(phoneNumber);

            string birthdate = updatedUserDTO.Birthdate;
            UserBusinessLogic.DefineBirthdateBL(birthdate);

            string username = updatedUserDTO.Username;
            UserBusinessLogic.DefineUsernameBL(username);

            string password = updatedUserDTO.Password;
            UserBusinessLogic.DefinePasswordBL(password);

            string answer = updatedUserDTO.SecurityAnswer;
            UserBusinessLogic.DefineAnswerBL(answer);

            string hashedPassword = "";
            _authenticationService.HashPassword(password, ref hashedPassword); // Encrypt Password (SHA256 Encryption)
            updatedUserDTO.Password = hashedPassword;

            string hashedAnswer = "";
            _authenticationService.HashPassword(answer, ref hashedAnswer); // Encrypt Password (SHA256 Encryption)
            updatedUserDTO.SecurityAnswer = hashedAnswer;

            User existingUser = _context.User.FirstOrDefault(u => u.Id == userId);
            bool emailChanged = false;

            if (existingUser == null)
            {
                throw new MyException("User not found");
            }

            if (updatedUserDTO.FirstName != null)
            {
                existingUser.FirstName = updatedUserDTO.FirstName;
            }

            if (updatedUserDTO.LastName != null)
            {
                existingUser.LastName = updatedUserDTO.LastName;
            }

            if (updatedUserDTO.Email != null)
            {
                if (updatedUserDTO.Email != existingUser.Email)
                    emailChanged = true;
                else
                    emailChanged = false;

                existingUser.Email = updatedUserDTO.Email;
            }

            if (updatedUserDTO.PhoneNumber != null)
            {
                existingUser.PhoneNumber = updatedUserDTO.PhoneNumber;
            }

            if (updatedUserDTO.Birthdate != null)
            {
                existingUser.Birthdate = updatedUserDTO.Birthdate;
            }

            if (updatedUserDTO.Username != null)
            {
                existingUser.Username = updatedUserDTO.Username;
            }

            if (updatedUserDTO.Password != null)
            {
                existingUser.Password = updatedUserDTO.Password;
            }

            if (updatedUserDTO.SecurityAnswer != null)
            {
                existingUser.SecurityAnswer = updatedUserDTO.SecurityAnswer;
            }

            if (emailChanged)
            {
                existingUser.EmailVerifiedAt = "";
                _context.SaveChanges();
                _authenticationService.SendEmailVerification(username, password);
            }
            else
            {
                _context.SaveChanges();
            }

            return _mapper.Map<RegisterUserDTO>(existingUser);
        }

       /*
        *  HTTP GET - Get User by Id
        */
        public RegisterUserDTO FindUserById(int id)
        {
            var usersList = _mapper.Map<IEnumerable<RegisterUserDTO>>(_context.User);
            var user = usersList.FirstOrDefault(x => x.Id == id);

            return user;
        }

        /*
         *  HTTP DELETE - Delete User
         */
        public void DeleteUserById(int id)
        {
            var userToDelete = _context.User.FirstOrDefault(u => u.Id == id);

            _context.User.Remove(userToDelete);
            _context.SaveChanges();
        }
    }
}