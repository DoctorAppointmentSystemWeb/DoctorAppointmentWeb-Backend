
using Microsoft.EntityFrameworkCore;
using DoctorAppointmentSystem.Models;


namespace DoctorAppointmentSystem.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<AppointmentReminder> AppointmentReminders { get; set; }
        public DbSet<DailySummary> DailySummaries { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }


      
        public DbSet<User> Users { get; set; }

       
        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<Doctor> Doctors { get; set; }

        
        public DbSet<DoctorAvailability> DoctorAvailabilities { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppointmentReminder>()
                .HasIndex(ar => ar.AppointmentId)
                .HasDatabaseName("IX_AppointmentReminders_AppointmentId");
            modelBuilder.Entity<DailySummary>()
                .HasIndex(ds => ds.SummaryDate)
                .HasDatabaseName("IX_DailySummaries_SummaryDate");

            modelBuilder.Entity<Specialty>()
                .HasMany(s => s.Doctors)
                .WithOne(d => d.Specialty)
                .HasForeignKey(d => d.SpecialtyId)
                .OnDelete(DeleteBehavior.Cascade);

            
            modelBuilder.Entity<DoctorAvailability>()
                .HasOne<Doctor>()
                .WithMany()
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);

            
            modelBuilder.Entity<Appointment>()
                .HasOne<Doctor>()
                .WithMany()
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            
            modelBuilder.Entity<Appointment>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

           
            modelBuilder.Entity<Appointment>()
                .HasIndex(a => new { a.DoctorId, a.AppointmentDate, a.SlotTime })
                .IsUnique();

            modelBuilder.Entity<Appointment>()
                .Property(a => a.Mode)
                .HasMaxLength(20)
                .IsRequired();

        }
    }
}