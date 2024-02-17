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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (!optionsBuilder.IsConfigured)
            {
                // Ath Server
                optionsBuilder.UseSqlServer(
                    "server=DESKTOP-FG9B3DG\\SQLEXPRESS;" +
                    "database=CinemaDatabase_Production;" +
                    "Integrated Security=True;" +
                    "MultipleActiveResultSets=True;" +
                    "TrustServerCertificate=True");
                
                // Panos Server
                /*optionsBuilder.UseSqlServer(
                    "server=DESKTOP-611K8P1\\SQLEXPRESS;" +
                    "database=CinemaDatabase;" +
                    "Integrated Security=True;" +
                    "MultipleActiveResultSets=True;" +
                    "TrustServerCertificate=True");*/
            }
        }
    }
}