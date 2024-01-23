using AutoMapper;
using Cinema.Models;
using CinemaData;
using CinemaStore.Business;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace CinemaStoreUnitTests
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<CinemaContext> _mockContext;
        private Mock<DbSet<User>> _mockSet;
        private Mock<IMapper> _mockMapper;
        private UserService _userService;

        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            _mockContext = new Mock<CinemaContext>();
            _mockSet = new Mock<DbSet<User>>();
            _mockContext.Setup(m => m.User).Returns(_mockSet.Object);
            _mockMapper = new Mock<IMapper>();
            _userService = new UserService(_mockContext.Object, _mockMapper.Object);
        }

        [Test]
        public void UserServiceIdTest()
        {
            // [1] To id πρέπει να είναι ίσο με 0
            UserDTO userDTO1 = new UserDTO()
            {
                Id = 1
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO1));
        }

        [Test]
        public void UserServiceNameTest()
        {
            // [1] Το πλήθος των χαρακτήρων πρέπει να είναι τουλάχιστον 3
            UserDTO userDTO1 = new UserDTO()
            {
                Id = 0,
                FirstName = "Ge",
            };
            UserDTO userDTO2 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Pr"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO1));
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO2));

            // [2] Το πλήθος των χαρακτήρων πρέπει να είναι το πολύ 15
            UserDTO userDTO3 = new UserDTO()
            {
                Id = 0,
                FirstName = "Georgeeeeeeeeeee",
            };
            UserDTO userDTO4 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Preeeeeeezerakos"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO3));
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO4));

            // [3] Πρέπει να περιέχει μόνο γράμματα
            UserDTO userDTO5 = new UserDTO()
            {
                Id = 0,
                FirstName = "George123",
            };
            UserDTO userDTO6 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos123"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO5));
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO6));

            // [4] Το 1ο γράμμα πρέπει να είναι κεφαλαίο
            UserDTO userDTO7 = new UserDTO()
            {
                Id = 0,
                FirstName = "george"
            };
            UserDTO userDTO8 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "prezerakos"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO7));
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO8));

            // [5] Τα γράμματα εκτός από το 1ο, πρέπει να είναι πεζά
            UserDTO userDTO9 = new UserDTO()
            {
                Id = 0,
                FirstName = "GeOrge",
            };
            UserDTO userDTO10 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "PrEzErakos"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO9));
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO10));

            // [6] Τα γράμματα πρέπει να είναι όλα λατινικά
            UserDTO userDTO11 = new UserDTO()
            {
                Id = 0,
                FirstName = "Γιώργος",
            };
            UserDTO userDTO12 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Πρεζεράκος"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO11));
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO12));
        }

        [Test]
        public void UserServiceEmailTest()
        {
            // [1] Το πλήθος των χαρακτήρων πρέπει να είναι τουλάχιστον 10
            UserDTO userDTO1 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "look.com"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO1));

            // [2] Το πλήθος των χαρακτήρων πρέπει να είναι το πολύ 25
            UserDTO userDTO2 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "gpreeeeeeeeeez123@outlook.com"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO2));

            // [3] Πρέπει να τελειώνει σε "@gmail.com" ή "@hotmail.com" ή "@outlook.com"
            UserDTO userDTO3 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "gprez_gmail.gr"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO3));

            // [4] Πρέπει να περιέχει τουλάχιστον 1 γράμμα
            UserDTO userDTO4 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "123@gmail.com"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO4));

            // [5] Δεν πρέπει να περιλαμβάνει άλλα ειδικά σύμβολα πέρα από το σύμβολο '@' και το σύμβολο '.'
            UserDTO userDTO5 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "g_prez@gmail.com"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO5));

            // [6] Δεν πρέπει να περιέχει χαρακτήρες διαφυγής
            UserDTO userDTO6 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "g prez@gmail.com"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO6));

            // [7] Τα γράμματα πρέπει να είναι όλα λατινικά
            UserDTO userDTO7 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "γπρεζ@gmail.com"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO7));
        }

        [Test]
        public void UserServicePhoneNumberTest()
        {
            // [1] Το πλήθος των χαρακτήρων πρέπει να είναι τουλάχιστον 10
            UserDTO userDTO1 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "gprez@gmail.com",
                PhoneNumber = "69712"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO1));

            // [2] Το πλήθος των χαρακτήρων πρέπει να είναι το πολύ 10
            UserDTO userDTO2 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "gprez@gmail.com",
                PhoneNumber = "6971234343889012"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO2));

            // [3] Πρέπει να περιέχει μόνο ψηφία
            UserDTO userDTO3 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "gprez@gmail.com",
                PhoneNumber = "697Ab_1599"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO3));

            // [4] Πρέπει να ξεκινάει από τα ψηφία 69
            UserDTO userDTO4 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "gprez@gmail.com",
                PhoneNumber = "5971346467"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO4));
        }

        [Test]
        public void UserServiceBirthdateTest()
        {
            // [1] Το πλήθος των χαρακτήρων πρέπει να είναι τουλάχιστον 10
            UserDTO userDTO1 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "gprez@gmail.com",
                PhoneNumber = "6971586860",
                Birthdate = "1967-4-13"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO1));

            // [2] Το πλήθος των χαρακτήρων πρέπει να είναι το πολύ 10
            UserDTO userDTO2 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "gprez@gmail.com",
                PhoneNumber = "6971586860",
                Birthdate = "1967-040-13"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO2));

            // [3.1] Η ημερομηνία πρέπει να περιλαμβάνει τον χαρακτήρα '-'
            UserDTO userDTO3 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "gprez@gmail.com",
                PhoneNumber = "6971586860",
                Birthdate = "1967/04/13"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO3));

            //  [3.2] Η ημερομηνία πρέπει να περιλαμβάνει μόνο ψηφία
            UserDTO userDTO4 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "gprez@gmail.com",
                PhoneNumber = "6971586860",
                Birthdate = "1967-April-13"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO4));

            // [3.3] Το μορφότυπο της ημερομηνίας πρέπει να είναι "ΧΧΧΧ-ΜΜ-ΗΗ"
            UserDTO userDTO5 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "gprez@gmail.com",
                PhoneNumber = "6971586860",
                Birthdate = "13-4-1967"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO5));

            // [3.4.1] Η εισαγωγή χρονιάς μεγαλύτερης του 2024 δεν είναι επιτρεπτή
            UserDTO userDTO6 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "gprez@gmail.com",
                PhoneNumber = "6971586860",
                Birthdate = "2100-04-31"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO6));

            // [3.4.2] Με εισαγωγή 04 στο πεδίο "ΜΜ", οι έγκυρες εισαγωγές στο πεδίο "ΗΗ" είναι από 1 εώς 30, καθώς ο μήνας Απρίλιος διαρκεί 30 μέρες
            UserDTO userDTO7 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "gprez@gmail.com",
                PhoneNumber = "6971586860",
                Birthdate = "1967-04-31"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO7));

            // [3.4.3] Με εισαγωγή 2003 στο πεδίο "ΧΧΧΧ" και 02 στο πεδίο "ΜΜ", οι έγκυρες εισαγωγές στο πεδίο "ΗΗ" είναι από 1 εώς 28, καθώς ο Φεβρουάριος διαρκεί 28 μέρες στα μη - δίσεκτα έτη
            UserDTO userDTO8 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "gprez@gmail.com",
                PhoneNumber = "6971586860",
                Birthdate = "1967-02-29"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO8));

            // [4.1] Η τελευταία επιτρεπτή ημερομηνία γέννησης πρέπει να είναι 2009-ΤρέχωνΜήνας-ΤρέχωνΗμέρα 
            UserDTO userDTO9 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "gprez@gmail.com",
                PhoneNumber = "6971586860",
                Birthdate = "2015-04-13"
            };
            UserDTO userDTO10 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "gprez@gmail.com",
                PhoneNumber = "6971586860",
                Birthdate = "2009-04-13"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO9));
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO10));

            // [5.1] Η πρώτη ημερομηνία γέννησης πρέπει να είναι 1933-ΤρέχωνΜήνας-(ΤρέχωνΗμέρα-1)
            UserDTO userDTO11 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "gprez@gmail.com",
                PhoneNumber = "6971586860",
                Birthdate = "1920-04-13"
            };
            UserDTO userDTO12 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "gprez@gmail.com",
                PhoneNumber = "6971586860",
                Birthdate = "1934-01-01"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO11));
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO12));
        }

        [Test]
        public void UserServiceUsernameTest()
        {
            // [1] Το πλήθος των χαρακτήρων πρέπει να είναι τουλάχιστον 8
            UserDTO userDTO1 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "gprez@gmail.com",
                PhoneNumber = "6971586860",
                Birthdate = "1967-04-13",
                Username = "gprez"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO1));

            // [2] Το πλήθος των χαρακτήρων πρέπει να είναι το πολύ 15
            UserDTO userDTO2 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "gprez@gmail.com",
                PhoneNumber = "6971586860",
                Birthdate = "1967-04-13",
                Username = "gpreeeeeeeeeeeez"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO2));

            // [3] Πρέπει να περιέχει τουλάχιστον 1 γράμμα
            UserDTO userDTO3 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "gprez@gmail.com",
                PhoneNumber = "6971586860",
                Birthdate = "1967-04-13",
                Username = "1234@67_9"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO3));

            // [4] Δεν πρέπει να περιέχει χαρακτήρες διαφυγής 
            UserDTO userDTO4 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "gprez@gmail.com",
                PhoneNumber = "6971586860",
                Birthdate = "1967-04-13",
                Username = "gprez 123"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO4));

            // [5] Τα γράμματα πρέπει να είναι όλα λατινικά
            UserDTO userDTO5 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "gprez@gmail.com",
                PhoneNumber = "6971586860",
                Birthdate = "1967-04-13",
                Username = "γπρεζ_123"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO5));
        }

        [Test]
        public void UserServicePasswordTest()
        {
            // [1] Το πλήθος των χαρακτήρων πρέπει να είναι τουλάχιστον 8
            UserDTO userDTO1 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "gprez@gmail.com",
                PhoneNumber = "6971586860",
                Birthdate = "1967-04-13",
                Username = "gprez_123",
                Password = "Uni_1"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO1));

            // [2] Το πλήθος των χαρακτήρων πρέπει να είναι το πολύ 15
            UserDTO userDTO2 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "gprez@gmail.com",
                PhoneNumber = "6971586860",
                Birthdate = "1967-04-13",
                Username = "gprez_123",
                Password = "Uniwa_0123456789"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO2));

            // [3] Πρέπει να περιέχει τουλάχιστον 1 κεφαλαίο γράμμα
            UserDTO userDTO3 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "gprez@gmail.com",
                PhoneNumber = "6971586860",
                Birthdate = "1967-04-13",
                Username = "gprez_123",
                Password = "uniwa_123"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO3));

            // [4] Πρέπει να περιέχει τουλάχιστον 1 πεζό γράμμα
            UserDTO userDTO4 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "gprez@gmail.com",
                PhoneNumber = "6971586860",
                Birthdate = "1967-04-13",
                Username = "gprez_123",
                Password = "UNIWA_123"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO4));

            // [5] Πρέπει να περιέχει τουλάχιστον 1 ψηφίο
            UserDTO userDTO5 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "gprez@gmail.com",
                PhoneNumber = "6971586860",
                Birthdate = "1967-04-13",
                Username = "gprez_123",
                Password = "Uniwa_only"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO5));

            // [6] Πρέπει να περιέχει τουλάχιστον 1 ειδικό σύμβολο ('@', '#', '$', '%', '^', '&', '_')
            UserDTO userDTO6 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "gprez@gmail.com",
                PhoneNumber = "6971586860",
                Birthdate = "1967-04-13",
                Username = "gprez_123",
                Password = "Uniwa1234"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO6));

            // [7] Δεν μπορεί να περιέχει χαρακτήρες διαφυγής
            UserDTO userDTO7 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "gprez@gmail.com",
                PhoneNumber = "6971586860",
                Birthdate = "1967-04-13",
                Username = "gprez_123",
                Password = "Uniwa_ 123"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO7));

            // [8] Τα γράμματα πρέπει να είναι όλα λατινικά
            UserDTO userDTO8 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "gprez@gmail.com",
                PhoneNumber = "6971586860",
                Birthdate = "1967-04-13",
                Username = "gprez_123",
                Password = "Παδά_1234"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO8));
        }
    }
}