using Cinema.Models;
using CinemaData;
using CinemaStore;
using CinemaStore.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace CinemaStoreIntegrationTests.Tests
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
                FirstName = "Lucas",
                LastName = "Roberts",
                Email = "arne39@ethereal.email",
                PhoneNumber = "6971654543",
                Birthdate = "1987-11-01",
                Username = "ArnoldVal",
                Password = "nsWP8EZeCuQtUSFkrd",
                ConfirmPassword = "nsWP8EZeCuQtUSFkrd"
            };

            var result = await TestUtilities.Post(client, route, registerUserDTO);
            var user = await ReadUser(result);

            var hashedPassword = "";
            HashPassword(registerUserDTO.Password, ref hashedPassword);
            registerUserDTO.Password = hashedPassword;

            Assert.True(user.Id > 0);
            Assert.True(user.FirstName == registerUserDTO.FirstName);
            Assert.True(user.LastName == registerUserDTO.LastName);
            Assert.True(user.Email == registerUserDTO.Email);
            Assert.True(user.PhoneNumber == registerUserDTO.PhoneNumber);
            Assert.True(user.Birthdate == registerUserDTO.Birthdate);
            Assert.True(user.Username == registerUserDTO.Username);
            Assert.True(user.Password == registerUserDTO.Password);
        }

        [Fact]
        public async Task VerifyEmail()
        {
            var route = "https://localhost:7236/API/Authentication/Verify-Email?token=";
            var client = _factory.CreateClient();

            var token = await RetrieveTokenFromDatabase();
            route += token;

            var result = await TestUtilities.Get(client, route);
            var verifyResult = await ReadEmailVerificationToken(result);

            Assert.True(verifyResult.Equals("Email verified successfully") || verifyResult.Equals("Email already verified"));
        }

        [Fact]
        public async Task Login()
        {
            var route = "https://localhost:7236/API/Authentication/Login";
            var client = _factory.CreateClient();

            // Admin Login
            LoginUserDTO loginUserDTO = new LoginUserDTO()
            {
                Username = "ArnoldVal",
                Password = "nsWP8EZeCuQtUSFkrd"
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

        private async Task<string> RetrieveTokenFromDatabase()
        {
            using (var dbContext = new CinemaContext())
            {
                var token = await dbContext.User
                    .OrderByDescending(t => t.Id)
                    .Select(t => t.EmailVerificationToken)
                    .FirstOrDefaultAsync();

                return token;
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