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
            // [3] Πρέπει να περιέχει μόνο γράμματα
            UserDTO userDTO1 = new UserDTO()
            {
                Id = 0,
                FirstName = "George123",
            };
            UserDTO userDTO2 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos123"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO1));
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO2));

            // [4] Το 1ο γράμμα πρέπει να είναι κεφαλαίο
            UserDTO userDTO3 = new UserDTO()
            {
                Id = 0,
                FirstName = "george"
            };
            UserDTO userDTO4 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "prezerakos"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO3));
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO4));

            // [5] Τα γράμματα εκτός από το 1ο, πρέπει να είναι πεζά
            UserDTO userDTO5 = new UserDTO()
            {
                Id = 0,
                FirstName = "GeOrge",
            };
            UserDTO userDTO6 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "PrEzErakos"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO5));
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO6));

            // [6] Τα γράμματα πρέπει να είναι όλα λατινικά
            UserDTO userDTO7 = new UserDTO()
            {
                Id = 0,
                FirstName = "Γιώργος",
            };
            UserDTO userDTO8 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Πρεζεράκος"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO5));
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO6));
        }

        [Test]
        public void UserServiceEmailTest()
        {
            // [3] Πρέπει να τελειώνει σε "@gmail.com" ή "@hotmail.com" ή "@outlook.com"
            UserDTO userDTO1 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "gprez_gmail.gr"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO1));

            // [4] Πρέπει να περιέχει τουλάχιστον 1 γράμμα
            UserDTO userDTO2 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "123@gmail.com"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO2));

            // [5] Δεν πρέπει να περιλαμβάνει άλλα ειδικά σύμβολα πέρα από το σύμβολο '@' και το σύμβολο '.'
            UserDTO userDTO3 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "g_prez@gmail.com"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO3));

            // [6] Δεν πρέπει να περιέχει χαρακτήρες διαφυγής
            UserDTO userDTO4 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "g prez@gmail.com"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO4));

            // [7] Τα γράμματα πρέπει να είναι όλα λατινικά
            UserDTO userDTO5 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "γπρεζ@gmail.com"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO5));
        }

        [Test]
        public void UserServicePhoneNumberTest()
        {
            // [3] Πρέπει να περιέχει μόνο ψηφία
            UserDTO userDTO1 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "gprez@gmail.com",
                PhoneNumber = "697Ab_1599"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO1));

            // [4] Πρέπει να ξεκινάει από τα ψηφία 69
            UserDTO userDTO2 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "gprez@gmail.com",
                PhoneNumber = "5971346467"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO2));
        }

        [Test]
        public void UserServiceBirthdateTest()
        {
            // [3.1] Η ημερομηνία πρέπει να περιλαμβάνει τον χαρακτήρα '-'
            UserDTO userDTO1 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "gprez@gmail.com",
                PhoneNumber = "6971586860",
                Birthdate = "1967/04/13"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO1));

            //  [3.2] Η ημερομηνία πρέπει να περιλαμβάνει μόνο ψηφία
            UserDTO userDTO2 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "gprez@gmail.com",
                PhoneNumber = "6971586860",
                Birthdate = "1967-April-13"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO2));

            // [3.3] Το μορφότυπο της ημερομηνίας πρέπει να είναι "ΧΧΧΧ-ΜΜ-ΗΗ"
            UserDTO userDTO3 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "gprez@gmail.com",
                PhoneNumber = "6971586860",
                Birthdate = "13-4-1967"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO3));

            // [3.4.1] Με εισαγωγή 04 στο πεδίο "ΜΜ", οι έγκυρες εισαγωγές στο πεδίο "ΗΗ" είναι από 1 εώς 30, καθώς ο μήνας Απρίλιος διαρκεί 30 μέρες
            UserDTO userDTO4 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "gprez@gmail.com",
                PhoneNumber = "6971586860",
                Birthdate = "1967-04-31"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO4));

            // [3.4.2] Με εισαγωγή 2003 στο πεδίο "ΧΧΧΧ" και 02 στο πεδίο "ΜΜ", οι έγκυρες εισαγωγές στο πεδίο "ΗΗ" είναι από 1 εώς 28, καθώς ο Φεβρουάριος διαρκεί 28 μέρες στα μη - δίσεκτα έτη
            UserDTO userDTO5 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "gprez@gmail.com",
                PhoneNumber = "6971586860",
                Birthdate = "1967-02-29"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO4));

            // [4] Ο χρόνος γέννησης "ΧΧΧΧ" πρέπει να είναι από 1924 εώς 2024
            UserDTO userDTO6 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "gprez@gmail.com",
                PhoneNumber = "6971586860",
                Birthdate = "1821-04-13"
            };
            UserDTO userDTO7 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "gprez@gmail.com",
                PhoneNumber = "6971586860",
                Birthdate = "2050-04-13"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO6));
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO7));

            // [5] Το απόρρητο ηλικίας είναι από 15 χρονών και πάνω
            UserDTO userDTO8 = new UserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "gprez@gmail.com",
                PhoneNumber = "6971586860",
                Birthdate = "2050-04-13"
            };
            Assert.Throws<MyException>(() => _userService.CreateUser(userDTO8));
        }
    }
}