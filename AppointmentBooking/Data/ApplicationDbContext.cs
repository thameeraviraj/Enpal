using AppointmentBooking.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AppointmentBooking.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<SalesManager> SalesManagers { get; set; }
        public DbSet<Slot> Slots { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SalesManager>()
                .Property(s => s.Languages)
                .HasColumnType("varchar[]");
            modelBuilder.Entity<SalesManager>()
               .Property(s => s.Products)
               .HasColumnType("varchar[]");
            modelBuilder.Entity<SalesManager>()
               .Property(s => s.CustomerRatings)
               .HasColumnType("varchar[]");
        }
    }
}
