using Cinema.Models;
using System.Text.RegularExpressions;

namespace CinemaStore.Business
{
    public static class UserBusinessLogic
    {
        /* FirstName & LastName :
         * 
         * [1] The number of characters must be at least 3
         * [2] Number of characters must be at most 15
         * [3] Must contain only letters
         * [4] The 1st letter must be capitalized
         * [5] All letters except the 1st letter must be in lower case
         * [6] Letters must be all Latin letters
         * 
         */
        public static void DefineNameBL(string name, string type)
        {
            // [1] The number of characters must be at least 3
            if (name.Length < 3)
            {
                throw new MyException("Error in " + type + " " +
                                     "[1] The number of characters must be at least 3");
            }

            // [2] Number of characters must be at most 15
            if (name.Length > 15)
            {
                throw new MyException("Error in " + type + " " +
                                     "[2] Number of characters must be at most 15");
            }

            // [3] Must contain only letters
            bool containsLetters = name.All(char.IsLetter);
            if (!containsLetters)
            {
                throw new MyException("Error in " + type + " " +
                                      "[3] Must contain only letters");
            }

            // [4] The 1st letter must be capitalized
            char[] nameArray = name.ToCharArray();
            bool isUpperFirstLetter = char.IsUpper(nameArray[0]);
            if (!isUpperFirstLetter)
            {
                throw new MyException(" " + type + " " +
                                      "[4] The 1st letter must be capitalized");
            }

            // [5] All letters except the 1st letter must be in lower case
            bool areLowerRestLetters = name.Substring(1).All(char.IsLower);
            if (!areLowerRestLetters)
            {
                throw new MyException("Error in " + type + " " +
                                      "[5] All letters except the 1st letter must be in lower case");
            }

            // [6] Letters must be all Latin letters
            bool isLatinLetter = true;
            int i;
            for (i = 0; i < nameArray.Length; i++)
            {
                isLatinLetter = (nameArray[i] >= 'A' && nameArray[i] <= 'Z') || (nameArray[i] >= 'a' && nameArray[i] <= 'z');
                if (!isLatinLetter)
                {
                    throw new MyException("Error in " + type + " " +
                                          "[6] Letters must be all Latin letters");
                }
            }
        }

        /* Email :
         * 
         * [1] The number of characters must be at least 10
         * [2] The number of characters must be at most 40
         * [3] Must end in "@ethereal.email
         * [4] Must contain at least 1 letter
         * [5] Letters must be all Latin letters 
         * 
         */
        public static void DefineEmailBL(string email)
        {
            // [1] The number of characters must be at least 10
            if (email.Length < 10)
            {
                throw new MyException("Error in email " +
                                     "[1] The number of characters must be at least 10");
            }

            // [2] The number of characters must be at most 40
            if (email.Length > 40)
            {
                throw new MyException("Error in email " +
                                     "[2] The number of characters must be at most 40);");
            }

            // [3] Must end in "@ethereal.email"
            bool isEmailFormat = email.EndsWith("@gmail.com") || email.EndsWith("@hotmail.com") || email.EndsWith("@outlook.com") || email.EndsWith("@ethereal.email");
            if (!isEmailFormat)
            {
                throw new MyException("Error in email " +
                                      "[3] Must end in \"@ethereal.email");
            }

            // [4] Must contain at least 1 lette
            string[] subEmails = email.Split('@');
            string emailUsername = subEmails[0];
            bool containsLetters = emailUsername.Any(char.IsLetter);
            if (!containsLetters)
            {
                throw new MyException("Error in email " +
                                      "[4] Must contain at least 1 letter");
            }

            // [5] Letters must be all Latin letters
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
                        throw new MyException("Error in email " +
                                              "[5] Letters must be all Latin letters");
                    }
                }
            }
        }

        /* PhoneNumber :
         * 
         * [1] The number of characters must be at least 10
         * [2] Number of characters must be at most 10
         * [3] Must contain only digits
         * [4] Must start with the digits 69
         *    [4.1] The telephone number must be a mobile number and from a Greek mobile phone company)
         * 
         */
        public static void DefinePhoneNumberBL(string phoneNumber)
        {
            // [1] The number of characters must be at least 10
            if (phoneNumber.Length < 10)
            {
                throw new MyException("Error in phone number " +
                                     "[1] The number of characters must be at least 10");
            }

            // [2] Number of characters must be at most 10
            if (phoneNumber.Length > 10)
            {
                throw new MyException("Error in phone number " +
                                     "[2] Number of characters must be at most 10");
            }

            // [3] Must contain only digits
            bool containsDigits = phoneNumber.All(char.IsDigit);
            if (!containsDigits)
            {
                throw new MyException("Error in phone number " +
                                      "[3] Must contain only digits");
            }

            // [4] Must start with the digits 69
            bool startsWith69 = phoneNumber.StartsWith("69");
            if (!startsWith69)
            {
                throw new MyException("Error in phone number " +
                                      "[4] Must start with the digits 69 " +
                                      "[4.1] The telephone number must be a mobile number and from a Greek mobile phone company)");
            }
        }

        /* Birthdate :
         *
         * [1] The number of characters must be at least 10
         * [2] Number of characters must be at least 10 
         * [3] The date must be valid
         *      [3.1] The date must contain the character '-' or '\' exactly 3 times
         *      [3.2] The date must contain only digits
         *      [3.3] The format of the date must be "XXX-MM-DD-HH"
         *      [3.4] The date of birth should be valid in terms of time, day and month correspondence
         *          [3.4.1] The entry of a year older than 2024 is not allowed
         *          [3.4.2] By entering 04 in the field 'MM', valid entries in the field 'HH' are from 1 to 30, as the month April lasts 30 days
         *          [3.4.3] By entering 2003 in field "XXX" and 02 in field "MM", valid entries in field "EH" are from 1 to 28, 
         *                  as February lasts 28 days in non-consecutive years
         * [4] The age limit starts from 15 years old
         *      [4.1] The last allowed date of birth must be 2009-CurrentMonth-CurrentDay
         * [5] The age limit ends at 90 years old
         *      [5.1] The first date of birth must be 1933-CurrentMonth-(CurrentDay-1)
         */
        public static void DefineBirthdateBL(string birthdate)
        {
            // [1] The number of characters must be at least 10
            if (birthdate.Length < 10)
            {
                throw new MyException("Error in birthdate " +
                                     "[1] The number of characters must be at least 10");
            }

            // [2] Number of characters must be at least 10
            if (birthdate.Length > 10)
            {
                throw new MyException("Error in birthdate " +
                                     "[2] Number of characters must be at least 10");
            }

            // [3] The date must be valid
            bool isValidDate = DateTime.TryParse(birthdate, out DateTime BirthDate);
            if (!isValidDate)
            {
                throw new MyException("Error in birthdate " +
                                      "[3] The date must be valid " +
                                      "[3.1] The date must contain the character '-' or '\' exactly 3 times " +
                                      "[3.2] The date must contain only digits " +
                                      "[3.3] The format of the date must be \"XXX-MM-DD-HH\" " +
                                      "[3.4] The date of birth should be valid in terms of time, day and month correspondence " +
                                      "[3.4.1] The entry of a year older than 2024 is not allowed " +
                                      "[3.4.2] By entering 04 in the field 'MM', valid entries in the field 'HH' are from 1 to 30, as the month April lasts 30 days " +
                                      "[3.4.3] By entering 2003 in field \"XXX\" and 02 in field \"MM\", valid entries in field \"EH\" are from 1 to 28, as February lasts 28 days in non-consecutive years");
            }


            // [4] The age limit starts from 15 years old
            bool is15yearsOld = (BirthDate.Year == 2009) && (BirthDate.Month <= DateTime.Today.Month) && (BirthDate.Day <= DateTime.Today.Day);
            bool is15yearsOldplus = (BirthDate.Year < 2009);
            if (!is15yearsOld && !is15yearsOldplus)
            {
                throw new MyException("Error in birthdate " +
                                      "[4] The age limit starts from 15 years old " +
                                      "[4.1] The last allowed date of birth must be 2009-CurrentMonth-CurrentDay");
            }

            // [5] The age limit ends at 90 years old
            bool is90yearsOld = (BirthDate.Year == 1934) && (BirthDate.Month > DateTime.Today.Month) && (BirthDate.Day > DateTime.Today.Day);
            bool is90yearsOldminus = (BirthDate.Year > 1934);
            if (!is90yearsOld && !is90yearsOldminus)
            {
                throw new MyException("Error in birthdate " +
                      "[5] The age limit ends at 90 years old " +
                      "[5.1] The first date of birth must be 1933-CurrentMonth-(CurrentDay-1)");
            }
        }

        /* Username :
         * 
         * [1] The number of characters must be at least 8
         * [2] The number of characters must be at most 30
         * [3] Must contain at least 1 letter
         * [4] Must not contain escape characters 
         * [5] The letters must all be in Latin
         * 
         */
        public static void DefineUsernameBL(string username)
        {
            // [1] The number of characters must be at least 8
            if (username.Length < 8)
            {
                throw new MyException("Error in username " +
                                     "[1] The number of characters must be at least 8");
            }

            // [2] The number of characters must be at most 30
            if (username.Length > 30)
            {
                throw new MyException("Error in username " +
                                     "[2] The number of characters must be at most 30");
            }

            // [3] Must contain at least 1 letter
            bool containsLetters = username.Any(char.IsLetter);
            if (!containsLetters)
            {
                throw new MyException("Error in username " +
                                      "[3] Must contain at least 1 letter");
            }

            // [4] Must not contain escape characters
            bool containsWhiteSpaces = username.Any(char.IsWhiteSpace);
            if (containsWhiteSpaces)
            {
                throw new MyException("Error in username " +
                                      "[4] Must not contain escape characters");
            }

            // [5] The letters must all be in Latin
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
                        throw new MyException("Error in username " +
                                              "[5] The letters must all be in Latin");
                    }
                }
            }
        }

        /* Password :
         * 
         * [1] The number of characters must be at least 8
         * [2] Number of characters must be at most 30
         * [3] Must contain at least 1 capital letter
         * [4] Must contain at least 1 lower case letter
         * [5] Must contain at least 1 digit
         * [6] May not contain escape characters
         * [7] All letters must be in Latin
         * 
         */
        public static void DefinePasswordBL(string password)
        {
            // [1] The number of characters must be at least 8
            if (password.Length < 8)
            {
                throw new MyException("Error in password " +
                                     "[1] The number of characters must be at least 8");
            }

            // [2] Number of characters must be at most 30
            if (password.Length > 30)
            {
                throw new MyException("Error in password " +
                                     "[2] Number of characters must be at most 30");
            }

            // [3] Must contain at least 1 capital letter
            bool containsUpperLetter = password.Any(char.IsUpper);
            if (!containsUpperLetter)
            {
                throw new MyException("Error in password " +
                                      "[3] Must contain at least 1 capital letter");
            }

            // [4] Must contain at least 1 lower case letter
            bool containsLowerLetter = password.Any(char.IsLower);
            if (!containsLowerLetter)
            {
                throw new MyException("Error in password " +
                                      "[4] Must contain at least 1 lower case letter");
            }

            // [5] Must contain at least 1 digit
            bool containsDigit = password.Any(char.IsDigit);
            if (!containsDigit)
            {
                throw new MyException("Error in password " +
                                      "[5] Must contain at least 1 digit");
            }

            // [6] May not contain escape characters
            bool containsWhiteSpaces = password.Any(char.IsWhiteSpace);
            if (containsWhiteSpaces)
            {
                throw new MyException("Error in password " +
                                      "[6] May not contain escape characters");
            }

            // [7] All letters must be in Latin
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
                        throw new MyException("Error in password " +
                                              "[8] All letters must be in Latin");
                    }
                }
            }
        }
    }
}