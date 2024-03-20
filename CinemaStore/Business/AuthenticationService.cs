using AutoMapper;
using Cinema.Models;
using CinemaData;
using CinemaData.Entities;
using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using System.Security.Cryptography;
using System.Text;
using MailKit.Net.Smtp;
using CinemaStore.Models;

namespace CinemaStore.Business
{
    public class AuthenticationService : IAuthenticationService
    {
        private CinemaContext _context;
        private IMapper _mapper;

        public AuthenticationService(CinemaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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

            string answer = registerUserDTO.SecurityAnswer;
            UserBusinessLogic.DefineAnswerBL(answer);

            string hashedPassword = "";
            this.HashPassword(password, ref hashedPassword); // Encrypt Password (Cipher's Encryption)
            registerUserDTO.Password = hashedPassword;
            registerUserDTO.ConfirmPassword = hashedPassword;

            string hashedAnswer = "";
            this.HashPassword(answer, ref hashedAnswer); // Encrypt Password (Cipher's Encryption)
            registerUserDTO.SecurityAnswer = hashedAnswer;
            

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

        
        // SHA256 Encryption
        public void HashPassword(string password, ref string hashedPassword)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(passwordBytes);
                hashedPassword = BitConverter.ToString(hashedBytes).Replace("-", "");
            }
        }

        private string GenerateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
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
                return "User not found";
            }

            User user = this._mapper.Map<User>(userDTO);

            string hashedPassword = "";
            this.HashPassword(loginUserDTO.Password, ref hashedPassword); // Encrypt Password (Cipher's Encryption)

            if (user.Password == hashedPassword)
            {
                return "Successful login";
            }
            else
            {
                return "Incorrect password";
            }
        }

        public RegisterUserDTO FindUserByUsername(string username)
        {
            var usersList = this._mapper.Map<IEnumerable<RegisterUserDTO>>(_context.User);
            var user = usersList.FirstOrDefault(x => x.Username == username);

            return user;
        }

        public string LoginWithAnswer(ForgotPWUserDTO forgotPWUserDTO)
        {
            var userDTO = this.FindUserByUsername(forgotPWUserDTO.Username);

            if (userDTO == null)
            {
                return "User not found";
            }

            User user = this._mapper.Map<User>(userDTO);

            string hashedAnswer = "";
            this.HashPassword(forgotPWUserDTO.SecurityAnswer, ref hashedAnswer); // Encrypt Password (Cipher's Encryption)

            if (user.SecurityAnswer == hashedAnswer)
            {
                return "Successful login";
            }
            else
            {
                return "Incorrect answer";
            }
        }
    }
}
