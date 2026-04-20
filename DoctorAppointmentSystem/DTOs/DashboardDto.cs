namespace DoctorAppointmentSystem.DTOs
{
    public class DashboardOverviewResponse
    {
        public int TotalAppointments { get; set; }
        public int ConfirmedAppointments { get; set; }
        public int CompletedAppointments { get; set; }
        public int CancelledAppointments { get; set; }
        public int NoShowAppointments { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalDoctors { get; set; }
        public int TotalPatients { get; set; }
    }

    public class RevenueByModeResponse
    {
        public string Mode { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
        public int AppointmentsCount { get; set; }
    }

    public class RevenueBySpecialtyResponse
    {
        public int SpecialtyId { get; set; }
        public string SpecialtyName { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
        public int AppointmentsCount { get; set; }
    }

    public class AppointmentTrendResponse
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
        public decimal Revenue { get; set; }
    }

    public class DoctorUtilizationResponse
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public int TotalAppointments { get; set; }
        public int CompletedAppointments { get; set; }
        public decimal Revenue { get; set; }
    }
}