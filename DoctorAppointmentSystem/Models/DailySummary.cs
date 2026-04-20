using System.ComponentModel.DataAnnotations;

namespace DoctorAppointmentSystem.Models
{
    public class DailySummary
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime SummaryDate { get; set; }

        public int? SpecialtyId { get; set; }

        [MaxLength(150)]
        public string? SpecialtyName { get; set; }

        [MaxLength(50)]
        public string? Mode { get; set; }

        public int TotalAppointments { get; set; }

        public int ConfirmedAppointments { get; set; }

        public int CompletedAppointments { get; set; }

        public int CancelledAppointments { get; set; }

        public int NoShowAppointments { get; set; }

        public decimal Revenue { get; set; }

        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    }
}