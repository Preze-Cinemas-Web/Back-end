using CinemaData.Entities;
using Microsoft.EntityFrameworkCore;

namespace CinemaData
{
    public class CinemaContext : DbContext
    {
        public CinemaContext()
        {
        }

        public CinemaContext(DbContextOptions<CinemaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Movie> Movie { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

                string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                var connectionString = "";

                if (environment == "Development")
                {
                    connectionString =
                        //"server=DESKTOP-4S64V8A\\SQLEXPRESS;" + // Spyros Server
                        //"server=DESKTOP-611K8P1\\SQLEXPRESS;" + // Panos Server
                        //"server=WIN-HNG469634LR;" +             // Stelios Server
                        "server=DESKTOP-FG9B3DG\\SQLEXPRESS;" +   // Ath Server
                        "database=CinemaDatabase_Development;" +
                        "Integrated Security=True;" +
                        "MultipleActiveResultSets=True;" +
                        "TrustServerCertificate=True";
                }
                else if (environment == "Production")
                {
                    connectionString =
                        //"server=DESKTOP-4S64V8A\\SQLEXPRESS;" + // Spyros Server
                        //"server=DESKTOP-611K8P1\\SQLEXPRESS;" + // Panos Server
                        //"server=WIN-HNG469634LR;" +             // Stelios Server
                        "server=DESKTOP-FG9B3DG\\SQLEXPRESS;" +   // Ath Server
                        "database=CinemaDatabase_Production;" +
                        "Integrated Security=True;" +
                        "MultipleActiveResultSets=True;" +
                        "TrustServerCertificate=True";

                }
                else if (environment == "Staging")
                {
                    connectionString =
                        //"server=DESKTOP-4S64V8A\\SQLEXPRESS;" + // Spyros Server
                        //"server=DESKTOP-611K8P1\\SQLEXPRESS;" + // Panos Server
                        //"server=WIN-HNG469634LR;" +             // Stelios Server
                        "server=DESKTOP-FG9B3DG\\SQLEXPRESS;" +   // Ath Server
                        "database=CinemaDatabase_Staging;" +
                        "Integrated Security=True;" +
                        "MultipleActiveResultSets=True;" +
                        "TrustServerCertificate=True";
                }
                else
                {
                    throw new Exception("Environment not set correctly. Check the launchSettings.json file.");
                }

                optionsBuilder.UseSqlServer(connectionString);
            }

    }
}