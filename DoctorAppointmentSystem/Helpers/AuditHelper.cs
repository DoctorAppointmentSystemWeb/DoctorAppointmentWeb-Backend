using System;

namespace DoctorAppointmentSystem.Helpers
{
    public static class AuditHelper
    {
        public static void Log(string message)
        {
            Console.WriteLine($"[AUDIT] {DateTime.Now} - {message}");
        }
    }
}