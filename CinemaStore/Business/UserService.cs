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

        private void ValidateUser(UserDTO userDTO, bool isNewUser)
        {

            if (userDTO == null)
            {
                throw new ArgumentNullException(nameof(userDTO));
            }

            if (!isNewUser && userDTO.Id <= 0)
            {
                throw new MyException("Λανθασμένο id. Το πεδίο Id πρέπει να είναι μεγαλύτερο από 0 για υπάρχοντα χρήστη.");
            }

            // Null Object Check
            

            // Id Check
            

            // FirstName Check
            string firstName = userDTO.FirstName;

            char[] nameArrayF = firstName.ToCharArray();
            bool containsLettersF = firstName.All(char.IsLetter);                      // Ελέγχω αν όλα είναι γράμματα
            bool upperFirstLetterF = char.IsUpper(nameArrayF[0]);                      // Ελέγχω αν το 1ο γράμμα είναι κεφαλαίο
            bool lowerRestLettersF = firstName.Substring(1).All(char.IsLower);         // Ελέγχω αν τα υπόλοιπα γράμματα είναι πεζά
            bool isLatinF = true;                                                      // Ελέγχω αν έχει μόνο λατινικά γράμματα
            int i;
            for (i = 0; i < nameArrayF.Length; i++)
            {
                isLatinF = (nameArrayF[i] >= 'A' && nameArrayF[i] <= 'Z') || (nameArrayF[i] >= 'a' && nameArrayF[i] <= 'z');
                if (!isLatinF)
                {
                    break;
                }
            }
            if (!containsLettersF)
            {
                throw new MyException("Λανθασμένο όνομα.\nΠρέπει να έχει μόνο γράμματα!!");
            }
            if (!upperFirstLetterF)
            {
                throw new MyException("Λανθασμένο όνομα.\nΠρέπει το 1ο γράμμα να είναι κεφαλαίο!!");
            }
            if (!lowerRestLettersF)
            {
                throw new MyException("Λανθασμένο όνομα.\nΠρέπει τα γράμματα εκτός από το 1ο να είναι πεζά!!");
            }
            if (!isLatinF)
            {
                throw new MyException("Λανθασμένο όνομα.\nΠρέπει τα γράμματα να είναι λατινικά!!");
            }

            // LastName Check
            string lastName = userDTO.LastName;

            char[] nameArrayL = lastName.ToCharArray();
            bool containsLettersL = lastName.All(char.IsLetter);                       // Ελέγχω αν όλα είναι γράμματα
            bool upperFirstLetterL = char.IsUpper(nameArrayL[0]);                      // Ελέγχω αν το 1ο γράμμα είναι κεφαλαίο
            bool lowerRestLettersL = lastName.Substring(1).All(char.IsLower);          // Ελέγχω αν τα υπόλοιπα γράμματα είναι πεζά
            bool isLatinL = true;                                                      // Ελέγχω αν έχει μόνο λατινικά γράμματα
            int j;
            for (j = 0; j < nameArrayL.Length; j++)
            {
                isLatinL = (nameArrayL[j] >= 'A' && nameArrayL[j] <= 'Z') || (nameArrayL[j] >= 'a' && nameArrayL[j] <= 'z');
                if (!isLatinL)
                {
                    break;
                }
            }
            if (!containsLettersL)
            {
                throw new MyException("Λανθασμένο επώνυμο.\nΠρέπει να έχει μόνο γράμματα!!");
            }
            if (!upperFirstLetterL)
            {
                throw new MyException("Λανθασμένο επώνυμο.\nΠρέπει το 1ο γράμμα να είναι κεφαλαίο!!");
            }
            if (!lowerRestLettersL)
            {
                throw new MyException("Λανθασμένο επώνυμο.\nΠρέπει τα γράμματα εκτός από το 1ο να είναι πεζά!!");
            }
            if (!isLatinL)
            {
                throw new MyException("Λανθασμένο επώνυμο.\nΠρέπει τα γράμματα να είναι λατινικά!!");
            }

            // Email Check
            string email = userDTO.Email;

            bool validEmail = email.EndsWith("@gmail.com") || email.EndsWith("@hotmail.com") || email.EndsWith("@outlook.com");
            if (!validEmail) 
            {
                throw new MyException("Λανθασμένο email.\nΠρέπει να τελειώνει σε @gmail.com ή @hotmail.com ή @outlook.com!!");
            }

            // PhoneNumber Check
            string phoneNumber = userDTO.PhoneNumber;

            bool validPhoneNumber = phoneNumber.All(char.IsDigit);
            if (!validPhoneNumber) 
            {
                throw new MyException("Λανθασμένο νούμερο τηλεφώνου.\nΠρέπει να περιλαμβάνει αυστηρά 10 ψηφία!!");
            }

            // BirthDate Check
            string birthdate = userDTO.Birthdate;

            bool isDate = DateTime.TryParse(birthdate, out DateTime BirthDate);

            if (!isDate)
            {
                throw new MyException("Λανθασμένη ημερομηνία γέννησης.\nΤο μορφότυπο της ημερομηνίας πρέπει να είναι ΧΧΧΧ-ΜΜ-ΗΗ!!");
            } 
            else
            {
                bool validYear = (BirthDate.Year >= 1924 && BirthDate.Year <= DateTime.Now.Year);   // Έγκυρες χρονολογίες από 1924 μέχρι 2024..
                bool validAge = (DateTime.Now.Year - BirthDate.Year >= 15);

                if (!validYear)
                {
                    throw new MyException("Λανθασμένη ημερομηνία γέννησης.\nΈγκυρες χρονολογίες από 1924 εώς 2024!!");
                }
                if (!validAge)
                {
                    throw new MyException("Παραβίαση απορρήτου ηλικίας.\nΜόνο ηλικίες 15+ επιτρέπονται για εγγραφή!!");
                }
            }

            // Username Check
            string username = userDTO.Username;

            bool containsLettersU = username.Any(char.IsLetter);
            bool containsDigitsU = username.Any(char.IsDigit);
            bool containsSpecialCharsU = username.Contains("&") ||  username.Contains("@") || username.Contains("#") 
                || username.Contains("$") || username.Contains("%") || username.Contains("^") || username.Contains("_");
            if (!containsLettersU)
            {
                throw new MyException("Λανθασμένο όνομα χρήστη.\nΠρέπει να περιέχει γράμματα!!");
            }
            if (!containsDigitsU)
            {
                throw new MyException("Λανθασμένο όνομα χρήστη.\nΠρέπει να περιέχει αριθμούς!!");
            }
            if (!containsSpecialCharsU)
            {
                throw new MyException("Λανθασμένο όνομα χρήστη.\nΠρέπει να περιέχει ειδικούς χαρακτήρες!!");
            }

            // Password Check
            string password = userDTO.Password;

            bool containsLettersP = password.Any(char.IsLetter);
            bool containsDigitsP = password.Any(char.IsDigit);
            bool containsSpecialCharsP = password.Contains("&") || password.Contains("@") || password.Contains("#")
               || password.Contains("$") || password.Contains("%") || password.Contains("^") || password.Contains("_");
            if (!containsLettersP)
            {
                throw new MyException("Λανθασμένος κωδικός πρόσβασης.\nΠρέπει να περιέχει γράμματα!!");
            }
            if (!containsDigitsP)
            {
                throw new MyException("Λανθασμένο κωδικός πρόσβασης.\nΠρέπει να περιέχει αριθμούς!!");
            }
            if (!containsSpecialCharsP)
            {
                throw new MyException("Λανθασμένο κωδικός πρόσβασης.\nΠρέπει να περιέχει ειδικούς χαρακτήρες!!");
            }

            User user = this._mapper.Map<User>(userDTO); // Μετατρέπω το UserDTO (Store) -> User (Data)

            _context.User.Add(user);
            _context.SaveChanges();

        }

        public UserDTO CreateUser(UserDTO userDTO)
        {
            int id = userDTO.Id;
            ValidateUser(userDTO, true);

            if (id > 0)
            {
                throw new MyException("Λανθασμένο id. Το πεδίο Id δεν πρέπει να συμπληρωθεί από τον client, πρέπει να παραμείνει 0!!");
            }
            

            User user = this._mapper.Map<User>(userDTO);
            _context.User.Add(user);
            _context.SaveChanges();

            return userDTO;
        }

        public UserDTO UpdateUser(UserDTO updatedUserDTO)
        {
            ValidateUser(updatedUserDTO, false);

            var existingUser = _context.User.FirstOrDefault(u => u.Id == updatedUserDTO.Id);

            if (existingUser != null)
            {
                _mapper.Map(updatedUserDTO, existingUser);

                _context.SaveChanges();
                return updatedUserDTO;
            }
            else
            {
                throw new MyException("Δεν βρέθηκε χρήστης για ενημέρωση.");
            }
        }




        public bool DeleteUserById(int id)
        {
            try
            {
                var userToDelete = _context.User.FirstOrDefault(u => u.Id == id);

                if (userToDelete != null)
                {
                    _context.User.Remove(userToDelete);
                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new MyException("Η διαγραφή απέτυχε.", ex);
            }
        }
    }
}
