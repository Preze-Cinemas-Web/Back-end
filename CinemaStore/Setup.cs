using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CinemaStore
{
    public static class Setup
    {
        public static string token;
        public static IConfiguration configRoot;

        public static void AdminLogin()
        {
            var userId = 1; // Add userId
            var role = "Admin"; // Add role
            var tokenJWT = GetToken(userId, role);
            var tokenHandler = new JwtSecurityTokenHandler();
            token = tokenHandler.WriteToken(tokenJWT);
        }

        private static JwtSecurityToken GetToken(int userId, string role) // Add userId and role parameters
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configRoot["JWT:Key"]));

            var token = new JwtSecurityToken(
                               issuer: configRoot["JWT:Issuer"],
                               audience: configRoot["JWT:Audience"],
                               expires: DateTime.Now.AddHours(3),
                               claims: new[]
                               {
                                   new Claim("userId", userId.ToString()), // Add userId claim
                                   new Claim("role", role) // Add role claim
                               },
                               signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                               );

            return token;
        }
    }
}
