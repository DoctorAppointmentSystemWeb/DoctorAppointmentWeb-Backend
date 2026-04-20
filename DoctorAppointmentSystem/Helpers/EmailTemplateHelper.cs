using DoctorAppointmentSystem.Models;
using System.Text;

namespace DoctorAppointmentSystem.Helpers
{
    public static class EmailTemplateHelper
    {
        public static string BuildReminderSubject(Appointment appointment)
        {
            return $"Reminder: Your {appointment.Mode} appointment is scheduled on {appointment.AppointmentDate:dd-MM-yyyy}";
        }

        public static string BuildReminderBody(Appointment appointment, string patientName, string doctorName)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"Hello {patientName},");
            sb.AppendLine();
            sb.AppendLine("This is a reminder for your upcoming doctor appointment.");
            sb.AppendLine();
            sb.AppendLine($"Doctor: {doctorName}");
            sb.AppendLine($"Date: {appointment.AppointmentDate:dd-MM-yyyy}");
            sb.AppendLine($"Time: {appointment.SlotTime}");
            sb.AppendLine($"Mode: {appointment.Mode}");
            sb.AppendLine();
            sb.AppendLine("Please be available on time.");
            sb.AppendLine();
            sb.AppendLine("Regards,");
            sb.AppendLine("Doctor Appointment System");

            return sb.ToString();
        }

        public static string BuildCancellationSubject(Appointment appointment)
        {
            return $"Appointment Cancelled - {appointment.AppointmentDate:dd-MM-yyyy}";
        }

        public static string BuildCancellationBody(Appointment appointment, string patientName, string doctorName)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"Hello {patientName},");
            sb.AppendLine();
            sb.AppendLine("Your appointment has been cancelled.");
            sb.AppendLine($"Doctor: {doctorName}");
            sb.AppendLine($"Date: {appointment.AppointmentDate:dd-MM-yyyy}");
            sb.AppendLine($"Time: {appointment.SlotTime}");
            sb.AppendLine($"Mode: {appointment.Mode}");
            sb.AppendLine();
            sb.AppendLine("Regards,");
            sb.AppendLine("Doctor Appointment System");

            return sb.ToString();
        }
    }
}