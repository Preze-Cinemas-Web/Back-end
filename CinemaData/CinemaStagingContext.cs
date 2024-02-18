using Microsoft.EntityFrameworkCore;

namespace CinemaData
{
    public class CinemaStagingContext : DbContext
    {
        public CinemaStagingContext()
        {
        }

        public CinemaStagingContext(DbContextOptions<CinemaStagingContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                if (environment == "Development")
                {
                    optionsBuilder.UseSqlServer(
                        //"server=DESKTOP-4S64V8A\\SQLEXPRESS;" + // Spyros Server
                        //"server=DESKTOP-611K8P1\\SQLEXPRESS;" + // Panos Server
                        "server=DESKTOP-FG9B3DG\\SQLEXPRESS;" +   // Ath Server
                        "database=CinemaDatabase_Development;" +
                        "Integrated Security=True;" +
                        "MultipleActiveResultSets=True;" +
                        "TrustServerCertificate=True");
                }
                else if (environment == "Production")
                {
                    optionsBuilder.UseSqlServer(
                        //"server=DESKTOP-4S64V8A\\SQLEXPRESS;" + // Spyros Server
                        //"server=DESKTOP-611K8P1\\SQLEXPRESS;" + // Panos Server
                        "server=DESKTOP-FG9B3DG\\SQLEXPRESS;" +   // Ath Server
                        "database=CinemaDatabase_Production;" +
                        "Integrated Security=True;" +
                        "MultipleActiveResultSets=True;" +
                        "TrustServerCertificate=True");
                }
                else if (environment == "Staging")
                {
                    optionsBuilder.UseSqlServer(
                        //"server=DESKTOP-4S64V8A\\SQLEXPRESS;" + // Spyros Server
                        //"server=DESKTOP-611K8P1\\SQLEXPRESS;" + // Panos Server
                        "server=DESKTOP-FG9B3DG\\SQLEXPRESS;" +   // Ath Server
                        "database=CinemaDatabase_Staging;" +
                        "Integrated Security=True;" +
                        "MultipleActiveResultSets=True;" +
                        "TrustServerCertificate=True");
                }
            }
        }
    }
}