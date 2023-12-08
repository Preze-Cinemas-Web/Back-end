/*
 *  Εδώ γίνεται η σύνδεση με την βάση  
 */
using Cinema.Models;
using Dapper;
using System.Data;

namespace Cinema.Helpers
{
    public class DatabaseHelper
    {
        private readonly IConfiguration _configuration;

        public DatabaseHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<Users> CreateUser(Users users)
        {
            string connectionString = _configuration.GetConnectionString("ConnectionWithDatabase");
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                return connection.Query<Users>("SELECT * FROM USERS;").ToList();
            }  

        }
    }
}
