using Letters.Data.Reception;
using Microsoft.EntityFrameworkCore;

namespace Letters.Data
{
    public class ReceptionContext : DbContext
    {
        public ReceptionContext(DbContextOptions<ReceptionContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Reception.Reception> Receptions { get; set; }

        public DbSet<Letter> Letters { get; set; }

        public DbSet<Recipient> Recipients { get; set; }

        public DbSet<Attach> Attaches { get; set; }

        public DbSet<LetterLog> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Letter>().HasKey(_ => _.Id);

            modelBuilder.Entity<Attach>().HasKey(_ => _.Id);
            modelBuilder.Entity<Attach>().HasOne(_ => _.Letter).WithMany(_ => _.Attaches).HasForeignKey(_ => _.LetterId);

            modelBuilder.Entity<LetterLog>().HasKey(_ => _.Id);
        }
    }
}
