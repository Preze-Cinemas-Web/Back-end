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
        public UpdateUserDTO ModifyUser(UpdateUserDTO updatedUserDTO)
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

            string hashedPassword = "";
            this.HashPassword(password, ref hashedPassword); // Encrypt Password (SHA256 Encryption)
            updatedUserDTO.Password = hashedPassword;

            User existingUser = _context.User.FirstOrDefault(u => u.Username == username);
            
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

            if (!updatedUserDTO.Equals(existingUser.Email))
            {
                existingUser.EmailVerifiedAt = "";
                _context.SaveChanges();
                SendEmailVerification(username, password);
            }

            _context.SaveChanges();

            return this._mapper.Map<UpdateUserDTO>(existingUser);
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

        public RegisterUserDTO FindUserById(int id)
        {
            var usersList = this._mapper.Map<IEnumerable<RegisterUserDTO>>(_context.User);
            var user = usersList.FirstOrDefault(x => x.Id == id);

            return user;
        }

        public RegisterUserDTO FindUserByEmail(string email)
        {
            var usersList = this._mapper.Map<IEnumerable<RegisterUserDTO>>(_context.User);
            var user = usersList.FirstOrDefault(x => x.Email == email);

            return user;
        }

        public string GenerateAndSetNewPassword(string email)
        {
            var user = _context.User.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            string newPassword = GenerateRandomPassword();
            string newHashedPassword = string.Empty;

            HashPassword(newPassword, ref newHashedPassword);

            user.Password = newHashedPassword;
            SendNewPasswordEmail(email, newPassword);

            _context.SaveChanges();

            return newPassword;
        }

        public string SendNewPasswordEmail(string email, string password)
        {
            var user = _context.User.FirstOrDefault(u => u.Email == email);

            

            var emailMime = new MimeMessage();
            emailMime.From.Add(MailboxAddress.Parse("prezecinems@ethereal.email"));
            emailMime.To.Add(MailboxAddress.Parse(user.Email));
            emailMime.Subject = "Password Change";
            emailMime.Body = new TextPart(TextFormat.Plain)
            {
                Text = "Your new password is: " + password
                + "\n\n" + "If you did not request this password change please contact us!"
                + "\n\n" + "Preze Cinems Development Team"
            };

            using var smtp = new SmtpClient();

            smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(user.Email, password);
            smtp.Send(emailMime);
            smtp.Disconnect(true);

            return "Email has been sent!" ;
        }




        private static readonly Random random = new Random();
        private string GenerateRandomPassword()
        {
            const string capitalLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowercaseLetters = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";

            const int minLength = 8;
            const int maxLength = 15;
            int totalCharacters = minLength + random.Next(maxLength - minLength + 1);

            var pool = new StringBuilder(capitalLetters)
            .Append(lowercaseLetters)
            .Append(digits);

            var password = new StringBuilder()
            .Append(GetRandomCharacter(capitalLetters))
            .Append(GetRandomCharacter(lowercaseLetters))
            .Append(GetRandomCharacter(digits));


            for (int i = 4; i < totalCharacters; i++)
            {
                password.Append(pool[random.Next(pool.Length)]);
            }

            for (int i = 0; i < password.Length; i++)
            {
                int index = random.Next(password.Length);
                char temp = password[i];
                password[i] = password[index];
                password[index] = temp;
            }

            return password.ToString();
        }

        private static char GetRandomCharacter(string source)
        {
            return source[random.Next(source.Length)];
        }
    }
}