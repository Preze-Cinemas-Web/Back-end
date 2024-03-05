using Cinema.Models;
using CinemaStore;
using CinemaStore.Business;
using CinemaStore.Models;
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

            RegisterUserDTO registerUserDTO = new RegisterUserDTO()
            {
                Id = 0,
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "gprez@gmail.com",
                PhoneNumber = "6944556563",
                Birthdate = "1995-03-01",
                Username = "dengamiesaileo",
                Password = "Prez_1234"
            };

            var result = await TestUtilities.Post(client, route, registerUserDTO);
            var user = await ReadUser(result);

            var hashedPassword = "";
            HashPassword(registerUserDTO.Password, ref hashedPassword);
            registerUserDTO.Password = hashedPassword;

            Assert.True(user.Id > 0);
            Assert.True(registerUserDTO.FirstName == user.FirstName);
            Assert.True(registerUserDTO.LastName == user.LastName);
            Assert.True(registerUserDTO.Email == user.Email);
            Assert.True(registerUserDTO.PhoneNumber == user.PhoneNumber);
            Assert.True(registerUserDTO.Birthdate == user.Birthdate);
            Assert.True(registerUserDTO.Username == user.Username);
            Assert.True(registerUserDTO.Password == user.Password);
        }

        
        [Fact]
        public async Task Login()
        {
            var route = "https://localhost:7236/API/Authentication/Login";
            var client = _factory.CreateClient();

            // Admin Login
            LoginUserDTO loginUserDTO = new LoginUserDTO()
            {
                Username = "prezerakus",
                Password = "Prez_1234"
            };

            var result = await TestUtilities.Post(client, route, loginUserDTO);
            var userToken = await ValidateUser(result);

            Assert.True(userToken != null);
        }

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

        private async Task<string> ValidateUser(HttpResponseMessage result)
        {
            Assert.True(result.IsSuccessStatusCode);
            
            return await result.Content.ReadAsStringAsync();
        }

    }
}