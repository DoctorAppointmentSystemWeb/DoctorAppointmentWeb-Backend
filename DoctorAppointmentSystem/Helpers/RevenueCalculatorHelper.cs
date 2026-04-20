using DoctorAppointmentSystem.Enum;
using DoctorAppointmentSystem.Models;

namespace DoctorAppointmentSystem.Helpers
{
    public static class RevenueCalculatorHelper
    {
        public static decimal CalculateRevenue(IEnumerable<Appointment> appointments)
        {
            return appointments
                .Where(a => a.Status != AppointmentStatus.Cancelled && a.Status != AppointmentStatus.NoShow)
                .Sum(a => a.ConsultationFee);
        }

        public static decimal CalculateCompletedRevenue(IEnumerable<Appointment> appointments)
        {
            return appointments
                .Where(a => a.Status == AppointmentStatus.Completed)
                .Sum(a => a.ConsultationFee);
        }
    }
}