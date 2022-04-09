using Letters.Data.Reception;
using Microsoft.EntityFrameworkCore;

namespace Letters.Data
{
    public class ReceptionContext : DbContext
    {
        public ReceptionContext(DbContextOptions<ReceptionContext> options) :base(options)
        {
           Database.EnsureCreated();
        }

        public DbSet<Reception.Reception> Receptions { get; set; }

        public DbSet<Letter> Letters { get; set; }

        public DbSet<Recipient> Recipients { get; set; }
    }
}
