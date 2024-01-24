using AutoMapper;
using Cinema.Models;
using CinemaData;
using Microsoft.AspNetCore.Mvc;

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

        public IEnumerable<UserDTO> GetAllUsers()
        {
            return this._mapper.Map<IEnumerable<UserDTO>>(_context.User);
        }

        public UserDTO GetUserById(int id)
        {
            var usersList = this._mapper.Map<IEnumerable<UserDTO>>(_context.User); // Μετατροπή από List<User> (Data) -> List<UserDTO> (Store)
            var user = usersList.FirstOrDefault(x => x.Id == id); // Βρες τον user με user.Id == id

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

        public bool Login(UserDTO userDTO)
        {
            User user = this._mapper.Map<User>(userDTO);
            bool isValid = ValidateLogin(user.Username, user.Password);
            if (isValid)
                return true;
            else
                return false;
        }

        private bool ValidateLogin(string username, string password)
        {
           var res = _context.User.SingleOrDefault(u => u.Username == username);
           if (res != null)
           {
                if (res.Password == password)
                {
                    return true;
                }
           }
           return false;
        }

        public UserDTO UpdateUser(UserDTO user)
        {
            throw new NotImplementedException();
        }

        public void DeleteUserById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
