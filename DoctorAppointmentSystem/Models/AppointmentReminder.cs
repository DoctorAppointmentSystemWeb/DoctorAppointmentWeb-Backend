using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorAppointmentSystem.Models
{
    public class AppointmentReminder
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AppointmentId { get; set; }

        [ForeignKey(nameof(AppointmentId))]
        public Appointment? Appointment { get; set; }

        [Required]
        public DateTime ReminderDateTime { get; set; }

        [Required]
        [MaxLength(50)]
        public string ReminderType { get; set; } = "Email";

        [Required]
        public bool IsSent { get; set; } = false;

        public DateTime? SentAt { get; set; }

        [MaxLength(255)]
        public string? EmailTo { get; set; }

        [MaxLength(255)]
        public string? Subject { get; set; }

        public string? Message { get; set; }

        [MaxLength(500)]
        public string? ErrorMessage { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}