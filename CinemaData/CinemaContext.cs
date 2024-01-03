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
                    "server=MSI\\SQLEXPRESS;" +
                    "database=CinemaDatabase;" +
                    "Integrated Security=True;" +
                    "MultipleActiveResultSets=True;" +
                    "TrustServerCertificate=True");
            }
        }
    }
}