using AutoMapper;
using Cinema.Models;
using CinemaData;
using CinemaStore.Models;
using System.Security.Cryptography;
using System.Text;


namespace CinemaStore.Business
{
    public class UserService : IUserService
    {
        private CinemaContext _context;
        private IMapper _mapper;

        public UserService(CinemaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable<RegisterUserDTO> FindAllUsers()
        {
            return this._mapper.Map<IEnumerable<RegisterUserDTO>>(_context.User);
        }

        public RegisterUserDTO FindUserById(int id)
        {
            var usersList = this._mapper.Map<IEnumerable<RegisterUserDTO>>(_context.User);
            var user = usersList.FirstOrDefault(x => x.Id == id);

            return user;
        }

        public RegisterUserDTO FindUserByUsername(string username)
        {
            var usersList = this._mapper.Map<IEnumerable<RegisterUserDTO>>(_context.User);
            var user = usersList.FirstOrDefault(x => x.Username == username);

            return user;
        }

        /*  HTTP POST - User
         *  
         *  Εδώ δημιουργούμε μία εγγραφή και θα γράψουμε τους περιορισμούς που συμφωνήσαμε να έχουν
         *  τα πεδία του User (π.χ. το email να έχει το format email @gmail.com).
         */
        public RegisterUserDTO Register(RegisterUserDTO registerUserDTO)
        {
            // Business Logic User

            UserBusinessLogic.DefineNullObjectBL(registerUserDTO);

            int id = registerUserDTO.Id;
            UserBusinessLogic.DefineIdBL(id);

            string firstName = registerUserDTO.FirstName;
            UserBusinessLogic.DefineNameBL(firstName, "όνομα");

            string lastName = registerUserDTO.LastName;
            UserBusinessLogic.DefineNameBL(lastName, "επώνυμο");

            string email = registerUserDTO.Email;
            UserBusinessLogic.DefineEmailBL(email);

            string phoneNumber = registerUserDTO.PhoneNumber;
            UserBusinessLogic.DefinePhoneNumberBL(phoneNumber);

            string birthdate = registerUserDTO.Birthdate;
            UserBusinessLogic.DefineBirthdateBL(birthdate);

            string username = registerUserDTO.Username;
            UserBusinessLogic.DefineUsernameBL(username);

            string password = registerUserDTO.Password;
            UserBusinessLogic.DefinePasswordBL(password);

            string hashedPassword = "";
            this.HashPassword(password, ref hashedPassword); // Encrypt Password (Cipher's Encryption)
            registerUserDTO.Password = hashedPassword;

            // Mapping to User & Insert into database

            User user = this._mapper.Map<User>(registerUserDTO);
            user.Role = "User";

            _context.User.Add(user);
            _context.SaveChanges();

            return _mapper.Map<RegisterUserDTO>(user);
        }

        public bool Login(LoginUserDTO loginUserDTO)
        {
            var userDTO = this.FindUserByUsername(loginUserDTO.Username);

            if (userDTO == null)
            {
                return false;
            }

            User user = this._mapper.Map<User>(userDTO);

            string hashedPassword = "";
            this.HashPassword(loginUserDTO.Password, ref hashedPassword); // Encrypt Password (Cipher's Encryption)

            return user.Password == hashedPassword;
        }

        // SHA256 Encryption
        private void HashPassword(string password, ref string hashedPassword)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(passwordBytes);
                hashedPassword = BitConverter.ToString(hashedBytes).Replace("-", "");
            }
        }

        public RegisterUserDTO UpdateUser(RegisterUserDTO UpdateduserDTO)
        {
            UserBusinessLogic.DefineNullObjectBL(UpdateduserDTO);

            int userId = UpdateduserDTO.Id;

            string firstName = UpdateduserDTO.FirstName;
            UserBusinessLogic.DefineNameBL(firstName, "First Name");

            string lastName = UpdateduserDTO.LastName;
            UserBusinessLogic.DefineNameBL(lastName, "Last Name");

            string email = UpdateduserDTO.Email;
            UserBusinessLogic.DefineEmailBL(email);

            string phoneNumber = UpdateduserDTO.PhoneNumber;
            UserBusinessLogic.DefinePhoneNumberBL(phoneNumber);

            string birthdate = UpdateduserDTO.Birthdate;
            UserBusinessLogic.DefineBirthdateBL(birthdate);

            string username = UpdateduserDTO.Username;
            UserBusinessLogic.DefineUsernameBL(username);

            string password = UpdateduserDTO.Password;
            UserBusinessLogic.DefinePasswordBL(password);

            string hashedPassword = "";
            this.HashPassword(password, ref hashedPassword); // Encrypt Password (SHA256 Encryption)
            UpdateduserDTO.Password = hashedPassword;

            User existingUser = _context.User.Find(UpdateduserDTO.Id);
            
            if (existingUser == null)
            {
                throw new Exception("Δεν βρέθηκε ο χρήστης.");
            }

            if (UpdateduserDTO.FirstName != null)
            {
                existingUser.FirstName = UpdateduserDTO.FirstName;
            }

            if (UpdateduserDTO.LastName != null)
            {
                existingUser.LastName = UpdateduserDTO.LastName;
            }

            if (UpdateduserDTO.Email != null)
            {
                existingUser.Email = UpdateduserDTO.Email;
            }

            if (UpdateduserDTO.PhoneNumber != null)
            {
                existingUser.PhoneNumber = UpdateduserDTO.PhoneNumber;
            }

            if (UpdateduserDTO.Birthdate != null)
            {
                existingUser.Birthdate = UpdateduserDTO.Birthdate;
            }

            if (UpdateduserDTO.Username != null)
            {
                existingUser.Username = UpdateduserDTO.Username;
            }

            if (UpdateduserDTO.Password != null)
            {
                existingUser.Password = UpdateduserDTO.Password;
            }

            _context.SaveChanges();

            return _mapper.Map<RegisterUserDTO>(existingUser);
        }

        public void DeleteUserById(int id)
        {
            var userToDelete = _context.User.FirstOrDefault(u => u.Id == id);

            _context.User.Remove(userToDelete);
            _context.SaveChanges();

        }
    }
}