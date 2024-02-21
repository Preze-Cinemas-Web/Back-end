using AutoMapper;
using Cinema.Models;
using CinemaData;
using CinemaStore.Models;


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
        public RegisterUserDTO Register(RegisterUserDTO userDTO)
        {
            // Business Logic User

            UserBusinessLogic.DefineNullObjectBL(userDTO);

            int id = userDTO.Id;
            UserBusinessLogic.DefineIdBL(id);

            string firstName = userDTO.FirstName;
            UserBusinessLogic.DefineNameBL(firstName, "όνομα");

            string lastName = userDTO.LastName;
            UserBusinessLogic.DefineNameBL(lastName, "επώνυμο");

            string email = userDTO.Email;
            UserBusinessLogic.DefineEmailBL(email);

            string phoneNumber = userDTO.PhoneNumber;
            UserBusinessLogic.DefinePhoneNumberBL(phoneNumber);

            string birthdate = userDTO.Birthdate;
            UserBusinessLogic.DefineBirthdateBL(birthdate);

            string username = userDTO.Username;
            UserBusinessLogic.DefineUsernameBL(username);

            string password = userDTO.Password;
            UserBusinessLogic.DefinePasswordBL(password);

            string hashedPassword = "";
            this.EncryptPassword(password, ref hashedPassword); // Encrypt Password (Cipher's Encryption)
            userDTO.Password = hashedPassword;

            // Mapping to User & Insert into database

            User user = this._mapper.Map<User>(userDTO);

            _context.User.Add(user);
            _context.SaveChanges();

            return _mapper.Map<RegisterUserDTO>(user);
        }

        public bool Login(LoginUserDTO oldUserDTO)
        {
            var userDTO = this.FindUserByUsername(oldUserDTO.Username);

            if (userDTO == null)
            {
                return false;
            }

            string hashedPassword = "";
            this.EncryptPassword(oldUserDTO.Password, ref hashedPassword); // Encrypt Password (Cipher's Encryption)

            User user = this._mapper.Map<User>(userDTO);

            return user.Password == hashedPassword;
        }

        // Cipher's Encryption
        private void EncryptPassword(string password, ref string hashedPassword)
        {
            int alphabetSize = 128;
            int shift = 5;

            foreach (char character in password)
            {
                char encryptedChar = (char)((character + shift) % alphabetSize);
                hashedPassword += encryptedChar;
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
            this.EncryptPassword(password, ref hashedPassword); // Encrypt Password (Cipher's Encryption)
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