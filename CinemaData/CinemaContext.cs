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
             
                optionsBuilder.UseSqlServer(
                    //"server=DESKTOP-611K8P1\\SQLEXPRESS;" + // Panos Server
                    //"server=DESKTOP-4S64V8A\\SQLEXPRESS;" + // Spyros Server
                    //"server=WIN-HNG469634LR;" +             // Stelios Server
                    "server=DESKTOP-FG9B3DG\\SQLEXPRESS;" +   // Ath Server
                    "database=CinemaDatabase;" +
                    "Integrated Security=True;" +
                    "MultipleActiveResultSets=True;" +
                    "TrustServerCertificate=True");

            }
        }
    }
}