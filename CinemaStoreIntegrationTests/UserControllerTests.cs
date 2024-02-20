using Cinema.Models;
using CinemaStore;
using CinemaStore.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Newtonsoft.Json;
using System.Runtime.ConstrainedExecution;
using Xunit;

namespace CinemaStoreIntegrationTests
{
    public class UserControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private WebApplicationFactory<Program> _factory;
        private const string EnvironmentVariable = "ASPNETCORE_ENVIRONMENT";

        public UserControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            Environment.SetEnvironmentVariable(EnvironmentVariable, "Staging");

        }

        [Fact]
        public async Task Register()
        {
            var route = "https://localhost:7236/API/Authentication/Register-User";
            var client = _factory.CreateClient();

            RegisterUserDTO userDTO = new RegisterUserDTO()
            {
                Id = 0,
                FirstName = "Yannis",
                LastName = "Alafouzos",
                Email = "alafou@gmail.com",
                PhoneNumber = "6944556563",
                Birthdate = "2000/03/01",
                Username = "ioannis12345",
                Password = "Alafouzos_123",
                Role = "User"
            };
           
            var result = await TestUtilities.Post(client, route, userDTO);
            var userDTO2 = await ReadUser(result);
            Assert.True(userDTO2.Id > 0);
            Assert.True(userDTO2.FirstName == userDTO.FirstName);
            Assert.True(userDTO2.LastName == userDTO.LastName);
            Assert.True(userDTO2.Email == userDTO.Email);
            Assert.True(userDTO2.PhoneNumber == userDTO.PhoneNumber);
            Assert.True(userDTO2.Birthdate == userDTO.Birthdate);
            Assert.True(userDTO2.Username == userDTO.Username);
            Assert.True(userDTO2.Password == userDTO.Password);
            Assert.True(userDTO2.Role == userDTO.Role);
        }

        [Fact]
        public async Task Login()
        {
            var route = "https://localhost:7236/API/1.0/User/Login";
            var client = _factory.CreateClient();

            LoginUserDTO oldUserDTO = new LoginUserDTO()
            {
                Username = "thpav123",
                Password = "Wiki_123"
            };

            var result = await TestUtilities.Post(client, route, oldUserDTO);
            var userDTO = await ReadUser(result);
            Assert.True(userDTO.Username == oldUserDTO.Username);
            Assert.True(userDTO.Password == oldUserDTO.Password);
        }

        private async Task<RegisterUserDTO> ReadUser(HttpResponseMessage result)
        {
            Assert.True(result.IsSuccessStatusCode);
            return JsonConvert.DeserializeObject<RegisterUserDTO>(
                await result.Content.ReadAsStringAsync());
        }
    }
}