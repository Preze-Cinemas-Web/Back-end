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

       /*  HTTP POST - User
        *  
        *  Εδώ δημιουργούμε μία εγγραφή και θα γράψουμε τους περιορισμούς που συμφωνήσαμε να έχουν
        *  τα πεδία του User (π.χ. το email να έχει το format email @gmail.com). 
        *  
        *  Business Logic User
        */
        public UserDTO CreateUser(UserDTO userDTO)
        {
            /*
             *  Null Object :
             */
            if (userDTO == null)
            {
                throw new ArgumentNullException(nameof(userDTO));
            }

            /* Id :
             * 
             * [1] To id πρέπει να είναι ίσο με 0
             * 
             */
            int id = userDTO.Id;
            
            if (id > 0) // [1] 
            {
                throw new MyException("Λανθασμένο id\n" +
                                      "[1] To id πρέπει να είναι ίσο με 0");
            }

            /* FirstName :
             * 
             * [1] Το πλήθος των χαρακτήρων πρέπει να είναι τουλάχιστον 3
		     * [2] Το πλήθος των χαρακτήρων πρέπει να είναι το πολύ 15
	         * [3] Πρέπει να περιέχει μόνο γράμματα
		     * [4] Το 1ο γράμμα πρέπει να είναι κεφαλαίο
		     * [5] Τα γράμματα εκτός από το 1ο, πρέπει να είναι πεζά
		     * [6] Τα γράμματα πρέπει να είναι όλα λατινικά
             * 
             */
            // [1] στο CinemaData/User.cs & CinemaStore/Models/UserDTO.cs
            // [2] στο CinemaData/User.cs & CinemaStore/Models/UserDTO.cs
            string firstName = userDTO.FirstName;
            
            char[] nameArrayF = firstName.ToCharArray(); 
            bool containsLettersF = firstName.All(char.IsLetter);              // [3]
            bool upperFirstLetterF = char.IsUpper(nameArrayF[0]);              // [4]
            bool lowerRestLettersF = firstName.Substring(1).All(char.IsLower); // [5] 
            bool isLatinF = true;                                              // [6]                                            
            int i;
            for (i = 0; i < nameArrayF.Length; i++)
            {
                isLatinF = (nameArrayF[i] >= 'A' && nameArrayF[i] <= 'Z') || (nameArrayF[i] >= 'a' && nameArrayF[i] <= 'z'); // [6]
                if (!isLatinF)                                                                                               // [6]
                {
                    break;
                }
            }
            if (!containsLettersF)  // [3]
            {
                throw new MyException("Λανθασμένο όνομα\n" +
                                      "[3] Πρέπει να περιέχει μόνο γράμματα");
            }
            if (!upperFirstLetterF) // [4]
            {
                throw new MyException("Λανθασμένο όνομα\n" +
                                      "[4] Το 1ο γράμμα πρέπει να είναι κεφαλαίο");
            }
            if (!lowerRestLettersF) // [5]
            {
                throw new MyException("Λανθασμένο όνομα\n" +
                                      "[5] Τα γράμματα εκτός από το 1ο, πρέπει να είναι πεζά");
            }
            if (!isLatinF)          // [6]
            {
                throw new MyException("Λανθασμένο όνομα\n" +
                                      "[6] Τα γράμματα πρέπει να είναι όλα λατινικά");
            }

            /* LastName :
             * 
             * [1] Το πλήθος των χαρακτήρων πρέπει να είναι τουλάχιστον 3
		     * [2] Το πλήθος των χαρακτήρων πρέπει να είναι το πολύ 15
	         * [3] Πρέπει να περιέχει μόνο γράμματα
		     * [4] Το 1ο γράμμα πρέπει να είναι κεφαλαίο
		     * [5] Τα γράμματα εκτός από το 1ο, πρέπει να είναι πεζά
		     * [6] Τα γράμματα πρέπει να είναι όλα λατινικά
             * 
             */
            // [1] στο CinemaData/User.cs & CinemaStore/Models/UserDTO.cs
            // [2] στο CinemaData/User.cs & CinemaStore/Models/UserDTO.cs
            string lastName = userDTO.LastName;

            char[] nameArrayL = lastName.ToCharArray();
            bool containsLettersL = lastName.All(char.IsLetter);                // [3]
            bool upperFirstLetterL = char.IsUpper(nameArrayL[0]);               // [4]       
            bool lowerRestLettersL = lastName.Substring(1).All(char.IsLower);   // [5]       
            bool isLatinL = true;                                               // [6]       
            int j;
            for (j = 0; j < nameArrayL.Length; j++)
            {
                isLatinL = (nameArrayL[j] >= 'A' && nameArrayL[j] <= 'Z') || (nameArrayL[j] >= 'a' && nameArrayL[j] <= 'z'); // [6]
                if (!isLatinL)                                                                                               // [6]
                {
                    break;
                }
            }
            if (!containsLettersL)  // [3]
            {
                throw new MyException("Λανθασμένο επώνυμο\n" +
                                      "[3] Πρέπει να περιέχει μόνο γράμματα");
            }
            if (!upperFirstLetterL) // [4]
            {
                throw new MyException("Λανθασμένο επώνυμο\n" +
                                      "[4] Το 1ο γράμμα πρέπει να είναι κεφαλαίο");
            }
            if (!lowerRestLettersL) // [5]
            {
                throw new MyException("Λανθασμένο επώνυμο\n" +
                                      "[5] Τα γράμματα εκτός από το 1ο, πρέπει να είναι πεζά");
            }
            if (!isLatinL)          // [6]
            {
                throw new MyException("Λανθασμένο επώνυμο\n" +
                                      "[6] Τα γράμματα πρέπει να είναι όλα λατινικά");
            }

            /* Email :
             * 
             * [1] Το πλήθος των χαρακτήρων πρέπει να είναι τουλάχιστον 10
             * [2] Το πλήθος των χαρακτήρων πρέπει να είναι το πολύ 25
             * [3] Πρέπει να τελειώνει σε "@gmail.com" ή "@hotmail.com" ή "@outlook.com"
             * [4] Πρέπει να περιέχει τουλάχιστον 1 γράμμα
             * [5] Δεν πρέπει να περιλαμβάνει άλλα ειδικά σύμβολα πέρα από το σύμβολο '@' και το σύμβολο '.' 
             * [6] Δεν πρέπει να περιέχει χαρακτήρες διαφυγής
             * [7] Τα γράμματα πρέπει να είναι όλα λατινικά
             * 
             */
            // [1] στο CinemaData/User.cs & CinemaStore/Models/UserDTO.cs
            // [2] στο CinemaData/User.cs & CinemaStore/Models/UserDTO.cs
            string email = userDTO.Email;

            bool emailFormat = email.EndsWith("@gmail.com") || email.EndsWith("@hotmail.com") || email.EndsWith("@outlook.com"); // [3]
            if (!emailFormat)                                                                                                    // [3]
            {
                throw new MyException("Λανθασμένο email\n" +
                                      "[3] Πρέπει να τελειώνει σε \"@gmail.com\" ή \"@hotmail.com\" ή \"@outlook.com\"");
            }
            string[] subEmails = email.Split('@');
            string emailName = subEmails[0];
            char[] emailNameArray = emailName.ToCharArray();
            bool containsLettersE = emailName.Any(char.IsLetter);         // [4]
            bool containsSymbolsE = emailName.Any(char.IsSymbol);         // [5]
            bool containsWhiteSpacesE = emailName.Any(char.IsWhiteSpace); // [6]
            bool isLatinE = true;                                         // [7]
            int w;
            for (w = 0; w < emailName.Length; w++)
            {
                if (char.IsLetter(emailName[w]))
                {
                    isLatinE = (emailName[w] >= 'A' && emailName[w] <= 'Z') || (emailName[w] >= 'a' && emailName[w] <= 'z'); // [7]
                    if (!isLatinE)                                                                                           // [7]
                    {
                        break;
                    }
                }
            }
            if (!containsLettersE)     // [4]
            {
                throw new MyException("Λανθασμένο email\n" +
                                      "[4] Πρέπει να περιέχει τουλάχιστον 1 γράμμα");
            }
            if (containsSymbolsE)      // [5]
            {
                throw new MyException("Λανθασμένο email\n" +
                                      "[5] Δεν πρέπει να περιλαμβάνει άλλα ειδικά σύμβολα πέρα από το σύμβολο '@' και το σύμβολο '.'");
            }
            if (containsWhiteSpacesE)  // [6]
            {
                throw new MyException("Λανθασμένο email\n" +
                                      "[6] Δεν πρέπει να περιέχει χαρακτήρες διαφυγής");
            }
            if (!isLatinE)             // [7]
            {
                throw new MyException("Λανθασμένο email\n" +
                                      "[7] Τα γράμματα πρέπει να είναι όλα λατινικά");
            }

            /* PhoneNumber :
             * 
             * [1] Το πλήθος των χαρακτήρων πρέπει να είναι τουλάχιστον 10
             * [2] Το πλήθος των χαρακτήρων πρέπει να είναι το πολύ 10
             * [3] Πρέπει να περιέχει μόνο ψηφία
             * [4] Πρέπει να ξεκινάει από τα ψηφία 69
               (Ο αριθμός τηλεφώνου πρέπει να είναι κινητό νούμερο και από ελληνική εταιρία κινητής τηλεφωνίας)
             * 
             */
            // [1] στο CinemaData/User.cs & CinemaStore/Models/UserDTO.cs
            // [2] στο CinemaData/User.cs & CinemaStore/Models/UserDTO.cs
            string phoneNumber = userDTO.PhoneNumber;

            bool containsDigitsP = phoneNumber.All(char.IsDigit); // [3]
            if (!containsDigitsP)                                 // [3]
            {
                throw new MyException("Λανθασμένος αριθμός τηλεφώνου\n" +
                                      "[3] Πρέπει να περιέχει μόνο ψηφία");
            }
            bool startsWith69 = phoneNumber.StartsWith("69"); // [4]
            if (!startsWith69)                                // [4]
            {
                throw new MyException("Λανθασμένος αριθμός τηλεφώνου\n" +
                                      "[4] Πρέπει να ξεκινάει από τα ψηφία 69");
            }

            /* Birthdate :
             *
             * [1] Το πλήθος των χαρακτήρων πρέπει να είναι τουλάχιστον 10
             * [2] Το πλήθος των χαρακτήρων πρέπει να είναι το πολύ 10
             * [3] Η ημερομηνία πρέπει να είναι έγκυρη
             *    [3.1] Η ημερομηνία πρέπει να περιλαμβάνει τον χαρακτήρα '-'
             *    [3.2] Η ημερομηνία πρέπει να περιλαμβάνει μόνο ψηφία
             *    [3.3] Το μορφότυπο της ημερομηνίας πρέπει να είναι "ΧΧΧΧ-ΜΜ-ΗΗ"
             *    [3.4] Η ημερομηνία γέννησης θα πρέπει να είναι έγκυρη ως προς την αντιστοιχία ημερών και μήνα
                  (παράδειγμα1: Με εισαγωγή 01 στο πεδίο "ΜΜ", οι έγκυρες εισαγωγές στο πεδίο "ΗΗ" είναι από 1 εώς 31, καθώς ο μήνας Ιανουάριος διαρκεί 31 μέρες)
                  (παράδειγμα2: Με εισαγωγή 2003 στο πεδίο "ΧΧΧΧ" και 02 στο πεδίο "ΜΜ", οι έγκυρες εισαγωγές στο πεδίο "ΗΗ" είναι από 1 εώς 28, 
                                καθώς ο Φεβρουάριος διαρκεί 28 μέρες στα μη-δίσεκτα έτη)
             * [4] Ο χρόνος γέννησης "ΧΧΧΧ" πρέπει να είναι από 1924 εώς 2024
             * [5] Το απόρρητο ηλικίας είναι από 15 χρονών και πάνω, δηλαδή επιτρεπτοί χρόνοι γέννησης βάση απορρήτου είναι από 1924 εώς 2009 
               (δεδομένου ότι η τρέχουσα χρονιά είναι 2024)
             */
            // [1] στο CinemaData/User.cs & CinemaStore/Models/UserDTO.cs
            // [2] στο CinemaData/User.cs & CinemaStore/Models/UserDTO.cs
            string birthdate = userDTO.Birthdate;

            bool isDate = DateTime.TryParse(birthdate, out DateTime BirthDate); // [3]

            if (!isDate) // [3]
            {
                throw new MyException("Λανθασμένη ημερομηνία γέννησης\n" +
                                      "[3] Η ημερομηνία πρέπει να είναι έγκυρη\n" +
                                      "   [3.1] Η ημερομηνία πρέπει να περιλαμβάνει τον χαρακτήρα '-'\n" +
                                      "   [3.2] Η ημερομηνία πρέπει να περιλαμβάνει μόνο ψηφία\n" +
                                      "   [3.3] Το μορφότυπο της ημερομηνίας πρέπει να είναι \"ΧΧΧΧ-ΜΜ-ΗΗ\"\n" +
                                      "   [3.4] Η ημερομηνία γέννησης θα πρέπει να είναι έγκυρη ως προς την αντιστοιχία ημερών και μήνα");
            } 
            else
            {
                bool validYear = (BirthDate.Year >= 1924 && BirthDate.Year <= DateTime.Now.Year);  // [4]
                bool validAge = (DateTime.Now.Year - BirthDate.Year >= 15);                        // [5]
                if (!validYear) // [4]
                {
                    throw new MyException("Λανθασμένη ημερομηνία γέννησης\n" +
                                          "[4] Ο χρόνος γέννησης \"ΧΧΧΧ\" πρέπει να είναι από 1924 εώς 2024");
                }
                if (!validAge)  // [5] 
                {
                    throw new MyException("Λανθασμένη ημερομηνία γέννησης\n" +
                                          "[5] Το απόρρητο ηλικίας είναι από 15 χρονών και πάνω, δηλαδή επιτρεπτοί χρόνοι γέννησης βάση απορρήτου είναι από 1924 εώς 2009");
                }
            }
            

           /* Username 
            * [1] Το πλήθος των χαρακτήρων πρέπει να είναι τουλάχιστον 8
		    * [2] Το πλήθος των χαρακτήρων πρέπει να είναι το πολύ 15
	        * [3] Πρέπει να περιέχει τουλάχιστον 1 γράμμα
		    * [4] Μπορεί να περιέχει το πολύ 1 ειδικό σύμβολο ('@', '#', '$', '%', '^', '&', '_')
		    * [5] Δεν πρέπει να περιέχει χαρακτήρες διαφυγής 
		    * [6] Τα γράμματα πρέπει να είναι όλα λατινικά
            */
            string username = userDTO.Username;

            char[] usernameArray = username.ToCharArray();
            bool containsLettersU = username.Any(char.IsLetter);                                                         // [3]
            bool containsSpecialCharsU = username.Contains("&") ||  username.Contains("@") || username.Contains("#") 
                || username.Contains("$") || username.Contains("%") || username.Contains("^") || username.Contains("_"); // [4]
            bool containsWhiteSpaceU = username.Any(char.IsWhiteSpace);                                                  // [5]
            bool isLatinU = true;                                                                                        // [6]                                            
            int k;
            for (k = 0; k < usernameArray.Length; k++)
            {
                if (char.IsLetter(usernameArray[k])) 
                {
                    isLatinU = (usernameArray[k] >= 'A' && usernameArray[k] <= 'Z') || (usernameArray[k] >= 'a' && usernameArray[k] <= 'z'); // [6]
                    if (!isLatinU)                                                                                                           // [6]
                    {
                        break;
                    }
                }

            }
            if (!containsLettersU)       // [3]
            {
                throw new MyException("Λανθασμένο όνομα χρήστη\n" +
                                      "[3] Πρέπει να περιέχει τουλάχιστον 1 γράμμα");
            }
            if (!containsSpecialCharsU)  // [4]
            {
                throw new MyException("Λανθασμένο όνομα χρήστη\n" +
                                      "[4] Μπορεί να περιέχει το πολύ 1 ειδικό σύμβολο ('@', '#', '$', '%', '^', '&', '_')");
            }
            if (containsWhiteSpaceU)     // [5]
            {
                throw new MyException("Λανθασμένο όνομα χρήστη\n" +
                                      "[5] Δεν πρέπει να περιέχει χαρακτήρες διαφυγής");
            }
            if (!isLatinU)               // [6]
            {
                throw new MyException("Λανθασμένο όνομα χρήστη\n" +
                                      "[6] Τα γράμματα πρέπει να είναι όλα λατινικά");
            }


            /* Password :
             * 
             * [1] Το πλήθος των χαρακτήρων πρέπει να είναι τουλάχιστον 8
		     * [2] Το πλήθος των χαρακτήρων πρέπει να είναι το πολύ 15
	         * [3] Πρέπει να περιέχει τουλάχιστον 1 κεφαλαίο γράμμα
		     * [4] Πρέπει να περιέχει τουλάχιστον 1 πεζό γράμμα
		     * [5] Πρέπει να περιέχει τουλάχιστον 1 ψηφίο
		     * [6] Πρέπει να περιέχει τουλάχιστον 1 ειδικό σύμβολο ('@', '#', '$', '%', '^', '&', '_')
		     * [7] Δεν μπορεί να περιέχει χαρακτήρες διαφυγής
		     * [8] Τα γράμματα πρέπει να είναι όλα λατινικά
             * 
             */
            string password = userDTO.Password;
            char[] passwordArray = password.ToCharArray();

            bool containsUpperLetterP = password.Any(char.IsUpper); // [3]
            bool containsLowerLetterP = password.Any(char.IsLower); // [4]
            bool containsDigitsPa = password.Any(char.IsDigit);     // [5]
            bool containsSpecialCharsP = password.Contains("&") || password.Contains("@") || password.Contains("#")
               || password.Contains("$") || password.Contains("%") || password.Contains("^") || password.Contains("_"); // [6]
            bool containsWhitespacesP = password.Any(char.IsWhiteSpace);                                                // [7]
            bool isLatinPa = true;                                                                                      // [8]
            int l;
            for (l = 0; l < passwordArray.Length; l++)
            {
                if (char.IsLetter(passwordArray[l]))
                {
                    isLatinPa = (passwordArray[l] >= 'A' && passwordArray[l] <= 'Z') || (passwordArray[l] >= 'a' && passwordArray[l] <= 'z'); // [8]
                    if (!isLatinPa)                                                                                                           // [8]
                    {
                        break;
                    }
                }
            }
            if (!containsUpperLetterP)  // [3]
            {
                throw new MyException("Λανθασμένος κωδικός πρόσβασης\n" +
                                      "[3] Πρέπει να περιέχει τουλάχιστον 1 κεφαλαίο γράμμα"); 
            }
            if (!containsLowerLetterP)  // [4]
            {
                throw new MyException("Λανθασμένος κωδικός πρόσβασης\n" +
                                      "[4] Πρέπει να περιέχει τουλάχιστον 1 πεζό γράμμα");
            }
            if (!containsDigitsPa)      // [5]
            {
                throw new MyException("Λανθασμένος κωδικός πρόσβασης\n" +
                                      "[5] Πρέπει να περιέχει τουλάχιστον 1 ψηφίο");
            }
            if (!containsSpecialCharsP) // [6]
            {
                throw new MyException("Λανθασμένος κωδικός πρόσβασης\n" +
                                      "[6] Πρέπει να περιέχει τουλάχιστον 1 ειδικό σύμβολο ('@', '#', '$', '%', '^', '&', '_')");
            }
            if (containsWhitespacesP) // [7]
            {
                throw new MyException("Λανθασμένος κωδικός πρόσβασης\n" +
                                      "[7] Δεν μπορεί να περιέχει χαρακτήρες διαφυγής");
            }
            if (!isLatinPa)            // [8]
            {
                throw new MyException("Λανθασμένος κωδικός πρόσβασης\n" +
                                      "[8] Τα γράμματα πρέπει να είναι όλα λατινικά");
            }

            User user = this._mapper.Map<User>(userDTO); // Μετατρέπω το UserDTO (Store) -> User (Data)

            _context.User.Add(user);
            _context.SaveChanges();

            return userDTO;
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
