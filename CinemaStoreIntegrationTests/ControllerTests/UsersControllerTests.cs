using Cinema.Models;
using CinemaStore;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace CinemaStoreIntegrationTests.ControllerTests
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

            RegisterUserDTO updateUserDTO = new RegisterUserDTO()
            {
                Id = 0,
                FirstName = "Tayler",
                LastName = "Kasselakis",
                Email = "autumn.hodkiewicz93@ethereal.email",
                PhoneNumber = "6971654543",
                Birthdate = "1987-11-01",
                Username = "ArnoldVlakas",
                Password = "FBwUDfQ7hzDEzNmKzJ",
                ConfirmPassword = "FBwUDfQ7hzDEzNmKzJ",
                SecurityAnswer = "Blue"
            };

            route += updateUserDTO.Username;
            var result = await TestUtilities.Put(client, route, updateUserDTO);
            var user = await ReadUser(result);

            var hashedPassword = "";
            HashPassword(updateUserDTO.Password, ref hashedPassword);
            updateUserDTO.Password = hashedPassword;

            var hashedAnswer = "";
            HashPassword(updateUserDTO.SecurityAnswer, ref hashedAnswer);
            updateUserDTO.SecurityAnswer = hashedAnswer;

            Assert.True(user.FirstName == updateUserDTO.FirstName);
            Assert.True(user.LastName == updateUserDTO.LastName);
            Assert.True(user.Email == updateUserDTO.Email);
            Assert.True(user.PhoneNumber == updateUserDTO.PhoneNumber);
            Assert.True(user.Birthdate == updateUserDTO.Birthdate);
            Assert.True(user.Username == updateUserDTO.Username);
            Assert.True(user.Password == updateUserDTO.Password);
            Assert.True(user.SecurityAnswer == updateUserDTO.SecurityAnswer);
        }

        [Fact]
        public async Task GerUserById()
        {
            var route = "https://localhost:7236/API/Users/Get-User-by-Id?id=";
            var client = _factory.CreateClient();

            int userId = 12;
            route += userId;

            var result = await TestUtilities.Get(client, route);
            var user = await ReadUser(result);

            Assert.True(user.Id == userId);
        }

        [Fact]
        public async Task GerUserByUsername()
        {
            var route = "https://localhost:7236/API/Users/Get-User-by-Username?username=";
            var client = _factory.CreateClient();

            string username = "ArnoldVlakas";
            route += username;

            var result = await TestUtilities.Get(client, route);
            var user = await ReadUser(result);

            Assert.True(user.Username == username);
        }

        [Fact]
        public async Task DeleteUser()
        {
            var route = "https://localhost:7236/API/Users/Delete?id=";
            var client = _factory.CreateClient();

            int userId = 13;
            route += userId;

            var result = await TestUtilities.Delete(client, route);

            Assert.True(result.IsSuccessStatusCode);
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

        private async Task<IEnumerable<RegisterUserDTO>> ReadUsers(HttpResponseMessage result)
        {
            Assert.True(result.IsSuccessStatusCode);
            string responseContent = await result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<IEnumerable<RegisterUserDTO>>(responseContent);
        }

        private async Task<RegisterUserDTO> ReadUser(HttpResponseMessage result)
        {
            Assert.True(result.IsSuccessStatusCode);
            string responseContent = await result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<RegisterUserDTO>(responseContent);
        }
    }
}
