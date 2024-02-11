using AutoMapper;
using Cinema.Models;
using CinemaData;
using CinemaStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


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

        public IEnumerable<UserDTO> FindAllUsers()
        {
            return this._mapper.Map<IEnumerable<UserDTO>>(_context.User);
        }

        public UserDTO FindUserById(int id)
        {
            var usersList = this._mapper.Map<IEnumerable<UserDTO>>(_context.User); // Μετατροπή από List<User> (Data) -> List<UserDTO> (Store)
            var user = usersList.FirstOrDefault(x => x.Id == id);                  // Βρες τον user με user.Id == id

            return user;
        }

        public UserDTO FindUserByUsername(string username)
        {
            var usersList = this._mapper.Map<IEnumerable<UserDTO>>(_context.User); // Μετατροπή από List<User> (Data) -> List<UserDTO> (Store)
            var user = usersList.FirstOrDefault(x => x.Username == username);      // Βρες τον user με user.Username == username

            return user;
        }

       /*  HTTP POST - User
        *  
        *  Εδώ δημιουργούμε μία εγγραφή και θα γράψουμε τους περιορισμούς που συμφωνήσαμε να έχουν
        *  τα πεδία του User (π.χ. το email να έχει το format email @gmail.com).
        */
        public UserDTO Register(UserDTO userDTO)
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
            
            // Mapping to User & Insert into database

            User user = this._mapper.Map<User>(userDTO); 

            _context.User.Add(user);
            _context.SaveChanges();

            return _mapper.Map<UserDTO>(user);
        }

        public bool Login(OldUserDTO oldUserDTO)
        {
            var userDTO = this.FindUserByUsername(oldUserDTO.Username); 
            
            if (userDTO == null)
            {
                return false;
            }

            User user = this._mapper.Map<User>(userDTO);

            return user.Password == oldUserDTO.Password;
        }

        public UserDTO UpdateUser(UserDTO userDTO)
        {
            UserBusinessLogic.DefineNullObjectBL(userDTO);

            int userId = userDTO.Id;

            string firstName = userDTO.FirstName;
            UserBusinessLogic.DefineNameBL(firstName, "First Name");

            string lastName = userDTO.LastName;
            UserBusinessLogic.DefineNameBL(lastName, "Last Name");

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

            User existingUser = _context.User.Find(userDTO.Id);
            if (existingUser == null)
            {
                throw new Exception("Δεν βρέθηκε ο χρήστης.");
            }




            if (userDTO.FirstName != null)
            {
                existingUser.FirstName = userDTO.FirstName;
            }

            if (userDTO.LastName != null)
            {
                existingUser.LastName = userDTO.LastName;
            }

            if (userDTO.Email != null)
            {
                existingUser.Email = userDTO.Email;
            }

            if (userDTO.PhoneNumber != null)
            {
                existingUser.PhoneNumber = userDTO.PhoneNumber;
            }

            if (userDTO.Birthdate != null)
            {
                existingUser.Birthdate = userDTO.Birthdate;
            }

            if (userDTO.Username != null)
            {
                existingUser.Username = userDTO.Username;
            }

            if (userDTO.Password != null)
            {
                existingUser.Password = userDTO.Password;
            }

            _context.SaveChanges();

            return _mapper.Map<UserDTO>(existingUser);
        }

        public void DeleteUserById(int id)
        {
            var userToDelete = _context.User.FirstOrDefault(u => u.Id == id);

            _context.User.Remove(userToDelete);
            _context.SaveChanges();

        }
    }
}
