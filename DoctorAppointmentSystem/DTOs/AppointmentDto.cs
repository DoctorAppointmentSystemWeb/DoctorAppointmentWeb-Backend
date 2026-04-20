using System;

namespace DoctorAppointmentSystem.DTOs
{
    public class AppointmentDto
    {
        public int DoctorId { get; set; }

        public int UserId { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan SlotTime { get; set; }

        public string Mode { get; set; } // Online / Offline
    }
}