using Cinema.Models;
using CinemaData;
using CinemaStore;
using CinemaStore.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace CinemaStoreIntegrationTests.ControllerTests
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
                FirstName = "Annetta",
                LastName = "Wyman",
                Email = "annetta53@ethereal.email",
                PhoneNumber = "6971654543",
                Birthdate = "1987-11-01",
                Username = "AnnetaKoita",
                Password = "P335qc37sRAYxZmCYJ",
                ConfirmPassword = "P335qc37sRAYxZmCYJ",
                SecurityAnswer = "Madonna"
            };

            var result = await TestUtilities.Post(client, route, registerUserDTO);
            var user = await ReadUser(result);

            var hashedPassword = "";
            HashPassword(registerUserDTO.Password, ref hashedPassword);
            registerUserDTO.Password = hashedPassword;

            var hashedAnswer = "";
            HashPassword(registerUserDTO.SecurityAnswer, ref hashedAnswer);
            registerUserDTO.SecurityAnswer = hashedAnswer;

            Assert.True(user.Id > 0);
            Assert.True(user.FirstName == registerUserDTO.FirstName);
            Assert.True(user.LastName == registerUserDTO.LastName);
            Assert.True(user.Email == registerUserDTO.Email);
            Assert.True(user.PhoneNumber == registerUserDTO.PhoneNumber);
            Assert.True(user.Birthdate == registerUserDTO.Birthdate);
            Assert.True(user.Username == registerUserDTO.Username);
            Assert.True(user.Password == registerUserDTO.Password);
            Assert.True(user.SecurityAnswer == registerUserDTO.SecurityAnswer);
        }

        [Fact]
        public async Task VerifyEmail()
        {
            var route = "https://localhost:7236/API/Authentication/Verify-Email?token=";
            var client = _factory.CreateClient();

            var token = "6E53269F7F1CFCA5FEBD69CFFD0956E4D560162D147837C31D49CA729DF885E532155B08BD1E586A74C825F7336248747B8DE584A4F1040D9B039DE374565BC4";
            route += token;

            var result = await TestUtilities.Get(client, route);
            var verifyResult = await ReadEmailVerificationToken(result);

            Assert.True(verifyResult == "Email verified successfully" || verifyResult == "Email already verified");
        }

        [Fact]
        public async Task Login()
        {
            var route = "https://localhost:7236/API/Authentication/Login";
            var client = _factory.CreateClient();

            LoginUserDTO loginUserDTO = new LoginUserDTO()
            {
                Username = "prezerak",
                Password = "GPrez_123123123123123123123123"
            };

            var result = await TestUtilities.Post(client, route, loginUserDTO);

            var token = await ValidateUser(result);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            var claims = jsonToken.Claims;

            Assert.NotNull(claims);

            int userId = 0;
            string role = " ";
            string username = " ";

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
                else if (claim.Type == "username")
                {
                    username = claim.Value;
                }
            }

            Assert.NotNull(Setup.token); // Check Admin's token
            Assert.True(userId == 1);
            Assert.True(role == "Admin");
            Assert.True(username == "prezerak");
        }

        [Fact]
        public async Task ForgotPassword()
        {
            var route = "https://localhost:7236/API/Authentication/Forgot-Password";
            var client = _factory.CreateClient();

            // Admin Login
            ForgotPWUserDTO forgotPWUserDTO = new ForgotPWUserDTO()
            {
                Username = "AnnetaKoita",
                SecurityAnswer = "Madonna"
            };

            var result = await TestUtilities.Post(client, route, forgotPWUserDTO);

            var token = await ValidateUser(result);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            var claims = jsonToken.Claims;

            Assert.NotNull(claims);

            int userId = 0;
            string role = " ";
            string username = " ";

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
                else if (claim.Type == "username")
                {
                    username = claim.Value;
                }
            }

            Assert.NotNull(Setup.token); // Check Admin's token
            Assert.True(userId > 1);
            Assert.True(role == "User");
            Assert.True(username == "AnnetaKoita");
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