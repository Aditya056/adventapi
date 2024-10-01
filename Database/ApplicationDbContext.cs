using advent_appointment_booking.Models;
using Microsoft.EntityFrameworkCore;

namespace advent_appointment_booking.Database
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<TruckingCompany> TruckingCompanies { get; set; }
        public DbSet<Terminal> Terminals { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Driver> Drivers { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Disable cascade delete to avoid multiple cascade paths / cycles
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.TruckingCompany)
                .WithMany(tc => tc.Appointments)
                .HasForeignKey(a => a.TrCompanyId)
                .OnDelete(DeleteBehavior.Restrict);  // Disable cascade delete

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Terminal)
                .WithMany(t => t.Appointments)
                .HasForeignKey(a => a.TerminalId)
                .OnDelete(DeleteBehavior.Restrict);  // Disable cascade delete

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Driver)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DriverId)
                .OnDelete(DeleteBehavior.Restrict);  // Disable cascade delete

            modelBuilder.Entity<Driver>()
                .HasOne(d => d.TruckingCompany)
                .WithMany(tc => tc.Drivers)
                .HasForeignKey(d => d.TrCompanyId)
                .OnDelete(DeleteBehavior.Restrict);  // Disable cascade delete
        }
    }
}
