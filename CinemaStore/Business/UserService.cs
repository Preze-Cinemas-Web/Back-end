using AutoMapper;
using Cinema.Models;
using CinemaData;

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

        public UserDTO CreateUser(UserDTO u)
        {
            /*
             *  Εδώ δημιουργούμε μία εγγραφή και θα γράψουμε τους περιορισμούς που συμφωνήσαμε να έχουν
             *  τα πεδία του user (π.χ. το email να έχει το format email @gmail.com)
             */
            User user = this._mapper.Map<User>(u); // Μετατρέπω το UserDTO (Store) -> User (Data)

            var result = _context.User.Add(user);
            _context.SaveChanges();

            return u;
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
