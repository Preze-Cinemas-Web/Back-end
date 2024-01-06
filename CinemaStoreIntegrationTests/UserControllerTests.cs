using CinemaStore;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace CinemaStoreIntegrationTests
{
    public class UserControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private WebApplicationFactory<Program> _factory;

        public UserControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        // [Fact]
        // ...
    }
}