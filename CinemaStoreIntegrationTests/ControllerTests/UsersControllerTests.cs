using Cinema.Models;
using CinemaStore;
using CinemaStore.Models;
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

        [Fact]
        public async Task UpdateUser()
        {
            var route = "https://localhost:7236/API/Users/Update?username=";
            var client = _factory.CreateClient();

            UpdateUserDTO updateUserDTO = new UpdateUserDTO()
            {
                FirstName = "Georgios",
                LastName = "Prezerakos",
                Email = "prezecinems@ethereal.email",
                PhoneNumber = "6971366764",
                Birthdate = "1970-07-29",
                Username = "prezerak",
                Password = "GPrez_123",
                ConfirmPassword = "GPrez_123"
            };

            route += updateUserDTO.Username;
            var result = await TestUtilities.Put(client, route, updateUserDTO);
            var user = await ReadUser(result);

            Assert.True(user.FirstName == updateUserDTO.FirstName);
            Assert.True(user.LastName == updateUserDTO.LastName);
            Assert.True(user.Email == updateUserDTO.Email);
            Assert.True(user.PhoneNumber == updateUserDTO.PhoneNumber);
            Assert.True(user.Birthdate == updateUserDTO.Birthdate);
            Assert.True(user.Username == updateUserDTO.Username);
            Assert.True(user.Password == updateUserDTO.Password);
        }
      
        private async Task<IEnumerable<RegisterUserDTO>> ReadUsers(HttpResponseMessage result)
        {
            Assert.True(result.IsSuccessStatusCode);
            string responseContent = await result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<IEnumerable<RegisterUserDTO>>(responseContent);
        }

        private async Task<UpdateUserDTO> ReadUser(HttpResponseMessage result)
        {
            Assert.True(result.IsSuccessStatusCode);
            string responseContent = await result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<UpdateUserDTO>(responseContent);
        }
    }
}
