namespace DoctorAppointmentSystem.DTOs
{
    public class SummaryFilterRequest
    {
        public DateTime Date { get; set; }
    }

    public class DailySummaryResponse
    {
        public DateTime SummaryDate { get; set; }
        public string? Mode { get; set; }
        public int? SpecialtyId { get; set; }
        public string? SpecialtyName { get; set; }
        public int TotalAppointments { get; set; }
        public int ConfirmedAppointments { get; set; }
        public int CompletedAppointments { get; set; }
        public int CancelledAppointments { get; set; }
        public int NoShowAppointments { get; set; }
        public decimal Revenue { get; set; }
    }

    public class ModeSummaryResponse
    {
        public string Mode { get; set; } = string.Empty;
        public int TotalAppointments { get; set; }
        public int ConfirmedAppointments { get; set; }
        public int CompletedAppointments { get; set; }
        public int CancelledAppointments { get; set; }
        public int NoShowAppointments { get; set; }
        public decimal Revenue { get; set; }
    }

    public class SpecialtySummaryResponse
    {
        public int SpecialtyId { get; set; }
        public string SpecialtyName { get; set; } = string.Empty;
        public int TotalAppointments { get; set; }
        public int ConfirmedAppointments { get; set; }
        public int CompletedAppointments { get; set; }
        public int CancelledAppointments { get; set; }
        public int NoShowAppointments { get; set; }
        public decimal Revenue { get; set; }
    }
}