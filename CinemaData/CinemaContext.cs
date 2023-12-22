using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "server=DESKTOP-FG9B3DG\\SQLEXPRESS;" +
                    "database=CinemaDB;" +
                    "Integrated Security=True;" +
                    "MultipleActiveResultSets=True;" +
                    "TrustServerCertificate=True");
            }
        }
    }
}
