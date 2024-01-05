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
                _ = optionsBuilder.UseSqlServer(
                    "server=DESKTOP-4S64V8A\\SQLEXPRESS;" +
                    "database=CinemaDatabase;" +
                    "Integrated Security=True;" +
                    "MultipleActiveResultSets=True;" +
                    "TrustServerCertificate=True",
                    b => b.MigrationsAssembly("CinemaStore"));
            }
        }
    }
}