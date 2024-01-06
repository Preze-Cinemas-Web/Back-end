using AutoMapper;
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

        // [Test]
        // ...
    }
}