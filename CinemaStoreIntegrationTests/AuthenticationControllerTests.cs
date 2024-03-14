using Cinema.Models;
using CinemaStore;
using CinemaStore.Business;
using CinemaStore.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CinemaStoreIntegrationTests
{
    public class AuthenticationControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private WebApplicationFactory<Program> _factory;
        private const string EnvironmentVariable = "ASPNETCORE_ENVIRONMENT";

        public AuthenticationControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            Environment.SetEnvironmentVariable(EnvironmentVariable, "Staging");

        }

        [Fact]
        public async Task Register()
        {
            var route = "https://localhost:7236/API/Authentication/Register";
            var client = _factory.CreateClient();

            RegisterUserDTO registerUserDTO = new RegisterUserDTO()
            {
                Id = 0,
                FirstName = "Meda",
                LastName = "Lemke",
                Email = "meda.lemke@ethereal.email",
                PhoneNumber = "6971654543",
                Birthdate = "1987-11-01",
                Username = "MedaLemke",
                Password = "gcVgrUDjhrescXmZ2s",
                ConfirmPassword = "gcVgrUDjhrescXmZ2s"
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
        public async Task VerifyEmail()
        {
            var verifyRoute = "https://localhost:7236/API/Authentication/Verify-Email?token=";
            var client = _factory.CreateClient();

            

            var verifyResult = await TestUtilities.Get(client, verifyRoute);
            var token = await ReadEmailVerificationToken(verifyResult);
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

            var token = await ValidateUser(result);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            var claims = jsonToken.Claims;

            Assert.NotNull(claims);

            int userId = 0;
            string role = " ";

            foreach (var claim in claims)
            {
                if (claim.Type == "userId")
                {
                    userId = int.Parse(claim.Value);
                }
                else if (claim.Type == "role")
                {
                    role = claim.Value;
                }
            }
            
            Assert.NotNull(Setup.token); // Check Admin's token
            Assert.True(userId > 1);
            Assert.True(role == "User");    
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

            return JsonConvert.DeserializeObject<RegisterUserDTO>(responseContent);
        }

        private async Task<string> ReadEmailVerificationToken(HttpResponseMessage result)
        {
            Assert.True(result.IsSuccessStatusCode);
            string responseContent = await result.Content.ReadAsStringAsync();

            return responseContent;
        }

        private async Task<string> ValidateUser(HttpResponseMessage result)
        {
            Assert.True(result.IsSuccessStatusCode);

            return await result.Content.ReadAsStringAsync();   
        }

    }
}