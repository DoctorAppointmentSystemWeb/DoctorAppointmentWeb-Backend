using System;
using DoctorAppointmentSystem.Enum;

namespace DoctorAppointmentSystem.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        public int DoctorId { get; set; }

        public int UserId { get; set; }

        public DateTime AppointmentDate { get; set; }

        public TimeSpan SlotTime { get; set; }

        public string Mode { get; set; } // Online / Offline

        public AppointmentStatus Status { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}