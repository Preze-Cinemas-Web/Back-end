using Cinema.Models;

namespace CinemaStore.Business
{
    public static class UserBusinessLogic
    {
       /*
        *  Null Object :
        */
        public static void DefineNullObjectBL(UserDTO userDTO)
        {
            if (userDTO == null)
            {
                throw new ArgumentNullException(nameof(userDTO));
            }
        }

       /* Id :
        * 
        * [1] To id πρέπει να είναι ίσο με 0
        * 
        */
        public static void DefineIdBL(int id)
        {
            /****** [1] ******/
            if (id != 0) 
            {
                throw new MyException("Λανθασμένο id\n" +
                                      "[1] To id πρέπει να είναι ίσο με 0");
            }
        }

        /* FirstName & LastName :
         * 
         * [1] Το πλήθος των χαρακτήρων πρέπει να είναι τουλάχιστον 3
		 * [2] Το πλήθος των χαρακτήρων πρέπει να είναι το πολύ 15
	     * [3] Πρέπει να περιέχει μόνο γράμματα
		 * [4] Το 1ο γράμμα πρέπει να είναι κεφαλαίο
		 * [5] Τα γράμματα εκτός από το 1ο, πρέπει να είναι πεζά
		 * [6] Τα γράμματα πρέπει να είναι όλα λατινικά
         * 
         */
        public static void DefineNameBL(string name, string type)
        {
            /******* [1] *******/

            // Η προδιαγραφή ελέγχεται στην κλάση CinemaData/User.cs & CinemaStore/Models/UserDTO.cs

            /******* [2] *******/

            // Η προδιαγραφή ελέγχεται στην κλάση CinemaData/User.cs & CinemaStore/Models/UserDTO.cs

            /******* [3] *******/
            bool containsLetters = name.All(char.IsLetter);
            if (!containsLetters)
            {
                throw new MyException("Λανθασμένο " + type + "\n" +
                                      "[3] Πρέπει να περιέχει μόνο γράμματα");
            }

            /******* [4] *******/
            char[] nameArray = name.ToCharArray();
            bool isUpperFirstLetter = char.IsUpper(nameArray[0]);          
            if (!isUpperFirstLetter)
            {
                throw new MyException("Λανθασμένο " + type + "\n" +
                                      "[4] Το 1ο γράμμα πρέπει να είναι κεφαλαίο");
            }

            /******* [5] *******/
            bool areLowerRestLetters = name.Substring(1).All(char.IsLower); 
            if (!areLowerRestLetters)
            {
                throw new MyException("Λανθασμένο " + type + "\n" +
                                      "[5] Τα γράμματα εκτός από το 1ο, πρέπει να είναι πεζά");
            }

            /******* [6] *******/
            bool isLatinLetter = true;                                                                              
            int i;
            for (i = 0; i < nameArray.Length; i++)
            {
                isLatinLetter = (nameArray[i] >= 'A' && nameArray[i] <= 'Z') || (nameArray[i] >= 'a' && nameArray[i] <= 'z'); 
                {
                    throw new MyException("Λανθασμένο " + type + "\n" +
                                          "[6] Τα γράμματα πρέπει να είναι όλα λατινικά");
                }
            }
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
        public static void DefineEmailBL(string email)
        {
            /******* [1] *******/

            // Η προδιαγραφή ελέγχεται στην κλάση CinemaData/User.cs & CinemaStore/Models/UserDTO.cs 

            /******* [2] *******/

            // Η προδιαγραφή ελέγχεται στην κλάση CinemaData/User.cs & CinemaStore/Models/UserDTO.cs

            /******* [3] *******/
            bool isEmailFormat = email.EndsWith("@gmail.com") || email.EndsWith("@hotmail.com") || email.EndsWith("@outlook.com");
            if (!isEmailFormat)                                                                                                 
            {
                throw new MyException("Λανθασμένο email\n" +
                                      "[3] Πρέπει να τελειώνει σε \"@gmail.com\" ή \"@hotmail.com\" ή \"@outlook.com\"");
            }

            /******* [4] *******/
            string[] subEmails = email.Split('@');
            string emailUsername = subEmails[0];
            bool containsLetters = emailUsername.Any(char.IsLetter);
            if (!containsLetters)
            {
                throw new MyException("Λανθασμένο email\n" +
                                      "[4] Πρέπει να περιέχει τουλάχιστον 1 γράμμα");
            }

            /******* [5] *******/
            bool containsSymbols = emailUsername.Any(char.IsSymbol);
            if (containsSymbols) 
            {
                throw new MyException("Λανθασμένο email\n" +
                                      "[5] Δεν πρέπει να περιλαμβάνει άλλα ειδικά σύμβολα πέρα από το σύμβολο '@' και το σύμβολο '.'");
            }

            /******* [6] *******/
            bool containsWhiteSpaces = emailUsername.Any(char.IsWhiteSpace);
            if (containsWhiteSpaces)
            {
                throw new MyException("Λανθασμένο email\n" +
                                      "[6] Δεν πρέπει να περιέχει χαρακτήρες διαφυγής");
            }

            /******* [7] *******/
            char[] emailNameArray = emailUsername.ToCharArray();        
            bool isLatinLetter = true;                          
            int i;
            for (i = 0; i < emailUsername.Length; i++)
            {
                if (char.IsLetter(emailUsername[i]))
                {
                    isLatinLetter = (emailUsername[i] >= 'A' && emailUsername[i] <= 'Z') || (emailUsername[i] >= 'a' && emailUsername[i] <= 'z'); 
                    if (!isLatinLetter)                                                                                           
                    {
                        throw new MyException("Λανθασμένο email\n" +
                                              "[7] Τα γράμματα πρέπει να είναι όλα λατινικά");
                    }
                }
            }
        }

       /* PhoneNumber :
        * 
        * [1] Το πλήθος των χαρακτήρων πρέπει να είναι τουλάχιστον 10
        * [2] Το πλήθος των χαρακτήρων πρέπει να είναι το πολύ 10
        * [3] Πρέπει να περιέχει μόνο ψηφία
        * [4] Πρέπει να ξεκινάει από τα ψηφία 69
             [4.1] Ο αριθμός τηλεφώνου πρέπει να είναι κινητό νούμερο και από ελληνική εταιρία κινητής τηλεφωνίας)
        * 
        */
        public static void DefinePhoneNumberBL(string phoneNumber)
        {
            /******* [1] *******/

            // Η προδιαγραφή ελέγχεται στην κλάση CinemaData/User.cs & CinemaStore/Models/UserDTO.cs 

            /******* [2] *******/

            // Η προδιαγραφή ελέγχεται στην κλάση CinemaData/User.cs & CinemaStore/Models/UserDTO.cs

            /******* [3] *******/
            bool containsDigits = phoneNumber.All(char.IsDigit); 
            if (!containsDigits)                              
            {
                throw new MyException("Λανθασμένος αριθμός τηλεφώνου\n" +
                                      "[3] Πρέπει να περιέχει μόνο ψηφία");
            }

            /******* [4] *******/
            bool startsWith69 = phoneNumber.StartsWith("69"); 
            if (!startsWith69)                                
            {
                throw new MyException("Λανθασμένος αριθμός τηλεφώνου\n" +
                                      "[4] Πρέπει να ξεκινάει από τα ψηφία 69\n" +
                                      "   [4.1] Ο αριθμός τηλεφώνου πρέπει να είναι κινητό νούμερο και από ελληνική εταιρία κινητής τηλεφωνίας");
            }
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
                    [3.4.1] Με εισαγωγή 04 στο πεδίο "ΜΜ", οι έγκυρες εισαγωγές στο πεδίο "ΗΗ" είναι από 1 εώς 30, καθώς ο μήνας Απρίλιος διαρκεί 30 μέρες
                    [3.4.2] Με εισαγωγή 2003 στο πεδίο "ΧΧΧΧ" και 02 στο πεδίο "ΜΜ", οι έγκυρες εισαγωγές στο πεδίο "ΗΗ" είναι από 1 εώς 28, 
                            καθώς ο Φεβρουάριος διαρκεί 28 μέρες στα μη-δίσεκτα έτη
         * [4] Ο χρόνος γέννησης "ΧΧΧΧ" πρέπει να είναι από 1924 εώς 2024
         * [5] Το απόρρητο ηλικίας είναι από 15 χρονών και πάνω
         *    [5.1] Ο χρόνος γέννησης "ΧΧΧΧ" πρέπει να είναι από 1924 εώς 2009
         */
        public static void DefineBirthdateBL(string birthdate)
        {
            /******* [1] *******/

            // Η προδιαγραφή ελέγχεται στην κλάση CinemaData/User.cs & CinemaStore/Models/UserDTO.cs 

            /******* [2] *******/

            // Η προδιαγραφή ελέγχεται στην κλάση CinemaData/User.cs & CinemaStore/Models/UserDTO.cs

            /******* [3] *******/
            bool isValidDate = DateTime.TryParse(birthdate, out DateTime BirthDate); 
            if (!isValidDate) 
            {
                throw new MyException("Λανθασμένη ημερομηνία γέννησης\n" +
                                      "[3] Η ημερομηνία πρέπει να είναι έγκυρη\n" +
                                      "   [3.1] Η ημερομηνία πρέπει να περιλαμβάνει τον χαρακτήρα '-'\n" +
                                      "   [3.2] Η ημερομηνία πρέπει να περιλαμβάνει μόνο ψηφία\n" +
                                      "   [3.3] Το μορφότυπο της ημερομηνίας πρέπει να είναι \"ΧΧΧΧ-ΜΜ-ΗΗ\"\n" +
                                      "   [3.4] Η ημερομηνία γέννησης θα πρέπει να είναι έγκυρη ως προς την αντιστοιχία ημερών και μήνα\n" +
                                      "        [3.4.1] Με εισαγωγή 01 στο πεδίο \"ΜΜ\", οι έγκυρες εισαγωγές στο πεδίο \"ΗΗ\" είναι από 1 εώς 31, καθώς ο μήνας Ιανουάριος διαρκεί 31 μέρες\n" +
                                      "        [3.4.2] Με εισαγωγή 2003 στο πεδίο \"ΧΧΧΧ\" και 02 στο πεδίο \"ΜΜ\", οι έγκυρες εισαγωγές στο πεδίο \"ΗΗ\" είναι από 1 εώς 28," +
                                      "                καθώς ο Φεβρουάριος διαρκεί 28 μέρες στα μη-δίσεκτα έτη");
            }

            /******* [4] *******/
            bool isValidYear = (BirthDate.Year >= 1924 && BirthDate.Year <= DateTime.Now.Year);
            if (!isValidYear) 
            {
                throw new MyException("Λανθασμένη ημερομηνία γέννησης\n" +
                                      "[4] Ο χρόνος γέννησης \"ΧΧΧΧ\" πρέπει να είναι από 1924 εώς 2024");
            }

            /******* [5] *******/
            bool isValidAge = (DateTime.Now.Year - BirthDate.Year >= 15);                      
            if (!isValidAge)   
            {
                throw new MyException("Λανθασμένη ημερομηνία γέννησης\n" +
                                      "[5] Το απόρρητο ηλικίας είναι από 15 χρονών και πάνω\n" +
                                      "   [5.1] Ο χρόνος γέννησης \"ΧΧΧΧ\" πρέπει να είναι από 1924 εώς 2009");
            }
        }

       /* Username :
        * 
        * [1] Το πλήθος των χαρακτήρων πρέπει να είναι τουλάχιστον 8
	    * [2] Το πλήθος των χαρακτήρων πρέπει να είναι το πολύ 15
	    * [3] Πρέπει να περιέχει τουλάχιστον 1 γράμμα
	    * [4] Δεν πρέπει να περιέχει χαρακτήρες διαφυγής 
	    * [5] Τα γράμματα πρέπει να είναι όλα λατινικά
        */
        public static void DefineUsernameBL(string username)
        {
            /******* [1] *******/

            // Η προδιαγραφή ελέγχεται στην κλάση CinemaData/User.cs & CinemaStore/Models/UserDTO.cs 

            /******* [2] *******/

            // Η προδιαγραφή ελέγχεται στην κλάση CinemaData/User.cs & CinemaStore/Models/UserDTO.cs

            /******* [3] *******/
            bool containsLetters = username.Any(char.IsLetter);
            if (!containsLetters)
            {
                throw new MyException("Λανθασμένο όνομα χρήστη\n" +
                                      "[3] Πρέπει να περιέχει τουλάχιστον 1 γράμμα");
            }

            /******* [4] *******/
            bool containsWhiteSpaces = username.Any(char.IsWhiteSpace);
            if (containsWhiteSpaces)
            {
                throw new MyException("Λανθασμένο όνομα χρήστη\n" +
                                      "[4] Δεν πρέπει να περιέχει χαρακτήρες διαφυγής");
            }

            /******* [5] *******/
            char[] usernameArray = username.ToCharArray();
            bool isLatinLetter = true;
            int i;
            for (i = 0; i < usernameArray.Length; i++)
            {
                if (char.IsLetter(usernameArray[i]))
                {
                    isLatinLetter = (usernameArray[i] >= 'A' && usernameArray[i] <= 'Z') || (usernameArray[i] >= 'a' && usernameArray[i] <= 'z');
                    if (!isLatinLetter)
                    {
                        throw new MyException("Λανθασμένο email\n" +
                                              "[5] Τα γράμματα πρέπει να είναι όλα λατινικά");
                    }
                }
            }
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
        public static void DefinePasswordBL(string password)
        {
            /******* [1] *******/

            // Η προδιαγραφή ελέγχεται στην κλάση CinemaData/User.cs & CinemaStore/Models/UserDTO.cs 

            /******* [2] *******/

            // Η προδιαγραφή ελέγχεται στην κλάση CinemaData/User.cs & CinemaStore/Models/UserDTO.cs

            /******* [3] *******/
            bool containsUpperLetter = password.Any(char.IsUpper);
            if (!containsUpperLetter) 
            {
                throw new MyException("Λανθασμένος κωδικός πρόσβασης\n" +
                                      "[3] Πρέπει να περιέχει τουλάχιστον 1 κεφαλαίο γράμμα");
            }

            /******* [4] *******/
            bool containsLowerLetter = password.Any(char.IsLower);
            if (!containsLowerLetter)  
            {
                throw new MyException("Λανθασμένος κωδικός πρόσβασης\n" +
                                      "[4] Πρέπει να περιέχει τουλάχιστον 1 πεζό γράμμα");
            }

            /******* [5] *******/
            bool containsDigit = password.Any(char.IsDigit);
            if (!containsDigit)
            {
                throw new MyException("Λανθασμένος κωδικός πρόσβασης\n" +
                                      "[5] Πρέπει να περιέχει τουλάχιστον 1 ψηφίο");
            }

            /******* [6] *******/
            bool containsSpecialChar = password.Contains("&") || password.Contains("@") || password.Contains("#")
               || password.Contains("$") || password.Contains("%") || password.Contains("^") || password.Contains("_");
            if (!containsSpecialChar) 
            {
                throw new MyException("Λανθασμένος κωδικός πρόσβασης\n" +
                                      "[6] Πρέπει να περιέχει τουλάχιστον 1 ειδικό σύμβολο ('@', '#', '$', '%', '^', '&', '_')");
            }

            /******* [7] *******/
            bool containsWhiteSpaces = password.Any(char.IsWhiteSpace);
            if (containsWhiteSpaces)
            {
                throw new MyException("Λανθασμένος κωδικός πρόσβασης\n" +
                                      "[7] Δεν μπορεί να περιέχει χαρακτήρες διαφυγής");
            }

            /******* [8] *******/
            char[] passwordArray = password.ToCharArray();
            bool isLatinLetter = true;                                                                                     
            int i;
            for (i = 0; i < passwordArray.Length; i++)
            {
                if (char.IsLetter(passwordArray[i]))
                {
                    isLatinLetter = (passwordArray[i] >= 'A' && passwordArray[i] <= 'Z') || (passwordArray[i] >= 'a' && passwordArray[i] <= 'z'); 
                    if (!isLatinLetter)                                                                                                           
                    {
                        throw new MyException("Λανθασμένος κωδικός πρόσβασης\n" +
                                              "[8] Τα γράμματα πρέπει να είναι όλα λατινικά");
                    }
                }
            }
        }
    }
}
