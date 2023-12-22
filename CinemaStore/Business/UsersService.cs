using AutoMapper;
using Cinema.Models;
using CinemaData;

namespace CinemaStore.Business
{
    public class UsersService : IUsersService
    {
        private CinemaContext _context;
        private IMapper _mapper;

        public UsersService(CinemaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable<UsersDTO> GetAllUsers()
        {
            return this._mapper.Map<IEnumerable<UsersDTO>>(_context.Users);
        }

        public UsersDTO GetUserById(int id)
        {
            var usersList = this._mapper.Map<IEnumerable<UsersDTO>>(_context.Users); // Μετατροπή από List<Users> (Data) -> List<UsersDTO> (Store)
            var user = usersList.FirstOrDefault(x => x.Id == id); // Βρες τον user με user.Id == id

            return user;
        }

        public UsersDTO CreateUser(UsersDTO u)
        {
            /*
             *  Εδώ δημιουργούμε μία εγγραφή και θα γράψουμε τους περιορισμούς που συμφωνήσαμε να έχουν
             *  τα πεδία του user (π.χ. το email να έχει το format email @gmail.com)
             */
            Users user = this._mapper.Map<Users>(u); // Μετατρέπω το UsersDTO (Store) -> Users (Data)

            var result = _context.Users.Add(user);
            _context.SaveChanges();

            return u;
        }

        public UsersDTO UpdateUser(UsersDTO user)
        {
            throw new NotImplementedException();
        }

        public void DeleteUserById(int id)
        {
            throw new NotImplementedException();
        }

        public UsersDTO UpdatePartialUser(UsersDTO user)
        {
            throw new NotImplementedException();
        }
    }
}
