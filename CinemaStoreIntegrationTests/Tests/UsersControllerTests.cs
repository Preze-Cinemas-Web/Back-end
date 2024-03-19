using Cinema.Models;
using CinemaStore;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;

namespace CinemaStoreIntegrationTests.Tests
{
    public class UsersControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private WebApplicationFactory<Program> _factory;
        private const string EnvironmentVariable = "ASPNETCORE_ENVIRONMENT";

        public UsersControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            Environment.SetEnvironmentVariable(EnvironmentVariable, "Staging");

        }

        [Fact]
        public async Task GetAllUsers()
        {
            var route = "https://localhost:7236/API/Users/Get-All-Users";
            var client = _factory.CreateClient();

            var result = await TestUtilities.Get(client, route);
            var users = await ReadUsers(result);

            Assert.True(users.Count() > 0);
        }

        private async Task<IEnumerable<RegisterUserDTO>> ReadUsers(HttpResponseMessage result)
        {
            Assert.True(result.IsSuccessStatusCode);
            string responseContent = await result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<IEnumerable<RegisterUserDTO>>(responseContent);
        }
    }
}
