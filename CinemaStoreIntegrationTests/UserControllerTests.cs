using Cinema.Models;
using CinemaStore;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Newtonsoft.Json;
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

        [Fact]
        public async Task Register()
        {
            var route = "https://localhost:7236/API/1.0/User/Register";
            var client = _factory.CreateClient();

            UserDTO userDTO = new UserDTO()
            {
                Id = 0,
                FirstName = "Theodosis",
                LastName = "Pavlidis",
                Email = "thpav13@gmail.com",
                PhoneNumber = "6971234345",
                Birthdate = "1962-02-01",
                Username = "thpav13",
                Password = "Wiki_123"
            };
           
            var result = await client.PostAsync(route, new StringContent(JsonConvert.SerializeObject(userDTO), System.Text.Encoding.UTF8, "application/json"));
            var userDTO2 = await ReadUser(result);
            Assert.True(userDTO2.Id > 0);
            Assert.True(userDTO2.FirstName == userDTO.FirstName);
            Assert.True(userDTO2.LastName == userDTO.LastName);
            Assert.True(userDTO2.Email == userDTO.Email);
            Assert.True(userDTO2.PhoneNumber == userDTO.PhoneNumber);
            Assert.True(userDTO2.Birthdate == userDTO.Birthdate);
            Assert.True(userDTO2.Username == userDTO.Username);
            Assert.True(userDTO2.Password == userDTO.Password);
        }

        private async Task<UserDTO> ReadUser(HttpResponseMessage result)
        {
            Assert.True(result.IsSuccessStatusCode);
            return JsonConvert.DeserializeObject<UserDTO>(
                await result.Content.ReadAsStringAsync());
        }   
    }
}