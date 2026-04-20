using Microsoft.EntityFrameworkCore;
using DoctorAppointmentSystem.Models;

namespace DoctorAppointmentSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // MEMBER 3 Tables
        public DbSet<DoctorAvailability> DoctorAvailabilities { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Prevent double booking at DB level (IMPORTANT)
            modelBuilder.Entity<Appointment>()
                .HasIndex(a => new { a.DoctorId, a.AppointmentDate, a.SlotTime })
                .IsUnique();

            // Optional: Better column precision
            modelBuilder.Entity<Appointment>()
                .Property(a => a.Mode)
                .HasMaxLength(20);
        }
    }
}