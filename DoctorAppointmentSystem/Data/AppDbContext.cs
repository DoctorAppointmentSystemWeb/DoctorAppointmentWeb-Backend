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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AppointmentReminder>()
                .HasIndex(ar => ar.AppointmentId)
                .HasDatabaseName("IX_AppointmentReminders_AppointmentId");
            modelBuilder.Entity<DailySummary>()
                .HasIndex(ds => ds.SummaryDate)
                .HasDatabaseName("IX_DailySummaries_SummaryDate");
        }
    }
}