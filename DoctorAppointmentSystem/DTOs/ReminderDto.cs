namespace DoctorAppointmentSystem.DTOs
{
    public class ReminderCreateRequest
    {
        public int AppointmentId { get; set; }
        public DateTime ReminderDateTime { get; set; }
        public string ReminderType { get; set; } = "Email";
    }

    public class ReminderSendRequest
    {
        public int ReminderId { get; set; }
    }

    public class ReminderResponse
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public DateTime ReminderDateTime { get; set; }
        public string ReminderType { get; set; } = string.Empty;
        public bool IsSent { get; set; }
        public DateTime? SentAt { get; set; }
        public string? EmailTo { get; set; }
        public string? Subject { get; set; }
        public string? Message { get; set; }
        public string? ErrorMessage { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}