using Cinema.Models;
using CinemaStore;
using CinemaStore.Business;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

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
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "gprez@gmail.com",
                PhoneNumber = "6944556563",
                Birthdate = "1995-03-01",
                Username = "paparaskaimisos",
                Password = "Prez_1234"
            };

            var result = await TestUtilities.Post(client, route, userDTO);
            var userDTO2 = await ReadUser(result);

            var hashedPassword = "";
            HashPassword(userDTO.Password, ref hashedPassword);
            userDTO.Password = hashedPassword;

            Assert.True(userDTO2.FirstName == userDTO.FirstName);
            Assert.True(userDTO2.LastName == userDTO.LastName);
            Assert.True(userDTO2.Email == userDTO.Email);
            Assert.True(userDTO2.PhoneNumber == userDTO.PhoneNumber);
            Assert.True(userDTO2.Birthdate == userDTO.Birthdate);
            Assert.True(userDTO2.Username == userDTO.Username);
            Assert.True(userDTO2.Password == userDTO.Password);
            Assert.True(userDTO2.Id > 0);
        }

        /*
        [Fact]
        public async Task Login()
        {
            var route = "https://localhost:7236/API/Authentication/Login";
            var client = _factory.CreateClient();
            
            // Admin Login
            LoginUserDTO userDTO = new LoginUserDTO()
            {
                Username = "prezerak",
                Password = "GPrez_123"
            }; 
        }  */

        private void HashPassword(string password, ref string hashedPassword)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(passwordBytes);
                hashedPassword = BitConverter.ToString(hashedBytes).Replace("-", "");
            }
        }

        private async Task<RegisterUserDTO> ReadUser(HttpResponseMessage result)
        {
            Assert.True(result.IsSuccessStatusCode);
            string responseContent = await result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<RegisterUserDTO>(
                    responseContent);
        }
    }
}