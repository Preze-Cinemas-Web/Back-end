using AutoMapper;
using Cinema.Models;
using CinemaData;
using CinemaStore.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
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

        /*
         * HTTP GET - Get All Users 
         */
        public IEnumerable<RegisterUserDTO> FindAllUsers()
        {
            return this._mapper.Map<IEnumerable<RegisterUserDTO>>(_context.User);
        }

        /*
         * HTTP POST - Register
         */
        public RegisterUserDTO Register(RegisterUserDTO registerUserDTO)
        {
            // Business Logic User

            string firstName = registerUserDTO.FirstName;
            UserBusinessLogic.DefineNameBL(firstName, "First Name");

            string lastName = registerUserDTO.LastName;
            UserBusinessLogic.DefineNameBL(lastName, "Last Name");

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
            registerUserDTO.ConfirmPassword = hashedPassword;

            // Mapping to User & Insert into database

            User user = this._mapper.Map<User>(registerUserDTO);
            user.Role = "User";
            var emailVerificationToken = GenerateRandomToken();

            // In case the token already exists, generate a new one
            while (_context.User.Any(u => u.EmailVerificationToken == emailVerificationToken))
            {
                emailVerificationToken = GenerateRandomToken();
            }
            user.EmailVerificationToken = emailVerificationToken;
            user.EmailVerifiedAt = "";

            _context.User.Add(user);
            _context.SaveChanges();

            return _mapper.Map<RegisterUserDTO>(user);
        }

        private string GenerateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
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

        /*
         * HTTP GET - Verify Email
         */
        public void UpdateVerificationDate(RegisterUserDTO userDTO)
        {
            var user = _context.User.Find(userDTO.Id);
            user.EmailVerifiedAt = DateTime.Now.ToString();
            _context.SaveChanges();
        }

        public string SendEmailVerification(string username, string password)
        {
            var user = _context.User.FirstOrDefault(u => u.Username == username);

            if (!user.EmailVerifiedAt.Equals(""))
            {
                return "Email already verified";
            }

            var emailMime = new MimeMessage();
            emailMime.From.Add(MailboxAddress.Parse("prezecinems@ethereal.email"));
            emailMime.To.Add(MailboxAddress.Parse(user.Email));
            emailMime.Subject = "Email Verification";
            emailMime.Body = new TextPart(TextFormat.Plain)
            {
                Text = "Please verify your email by clicking the link below: \n\n" + "https://localhost:7236/API/Authentication/Verify-Email?token=" + user.EmailVerificationToken
                + "\n\n" + "Preze Cinems Development Team"
            };

            using var smtp = new SmtpClient();

            smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(user.Email, password);
            smtp.Send(emailMime);
            smtp.Disconnect(true);

            return "Email sent for verification";
        }

        public RegisterUserDTO FindUserByEmailVerificationToken(string token)
        {
            var user = _context.User.FirstOrDefault(u => u.EmailVerificationToken == token);
            var userDTO = _mapper.Map<RegisterUserDTO>(user);

            return userDTO;
        }

        public bool isEmailVerified(string token)
        {
            var user = _context.User.FirstOrDefault(u => u.EmailVerificationToken == token);

            if (user.EmailVerifiedAt.Equals(""))
            {
                return false;
            }

            return true;
        }

        /*
         * HTTP POST - Login
         */
        public string Login(LoginUserDTO loginUserDTO)
        {
            var userDTO = this.FindUserByUsername(loginUserDTO.Username);

            if (userDTO == null)
            {
               return "Ο χρήστης δεν βρέθηκε";
            }

            User user = this._mapper.Map<User>(userDTO);

            string hashedPassword = "";
            this.HashPassword(loginUserDTO.Password, ref hashedPassword); // Encrypt Password (Cipher's Encryption)

            if (user.Password == hashedPassword)
            {
                return "Επιτυχής σύνδεση";
            }
            else
            {
                return "Λάθος κωδικός";
            }
        }

        public RegisterUserDTO FindUserByUsername(string username)
        {
            var usersList = this._mapper.Map<IEnumerable<RegisterUserDTO>>(_context.User);
            var user = usersList.FirstOrDefault(x => x.Username == username);

            return user;
        }

        /*
         * HTTP PUT - Update User
         */
        public RegisterUserDTO UpdateUser(RegisterUserDTO UpdateduserDTO)
        {
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

            if (!UpdateduserDTO.Equals(existingUser.Email))
            {
                existingUser.EmailVerifiedAt = "";
                _context.SaveChanges();
                SendEmailVerification(username, password);
            }

            _context.SaveChanges();

            return _mapper.Map<RegisterUserDTO>(existingUser);
        }

        public RegisterUserDTO FindUserById(int id)
        {
            var usersList = this._mapper.Map<IEnumerable<RegisterUserDTO>>(_context.User);
            var user = usersList.FirstOrDefault(x => x.Id == id);

            return user;
        }

        /*
         * HTTP DELETE - Delete User
         */
        public void DeleteUserById(int id)
        {
            var userToDelete = _context.User.FirstOrDefault(u => u.Id == id);

            _context.User.Remove(userToDelete);
            _context.SaveChanges();

        }
    }
}