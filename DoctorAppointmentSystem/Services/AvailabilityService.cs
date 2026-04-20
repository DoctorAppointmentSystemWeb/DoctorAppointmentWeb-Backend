using System;
using System.Collections.Generic;
using System.Linq;
using DoctorAppointmentSystem.Data;
using DoctorAppointmentSystem.Helpers;

namespace DoctorAppointmentSystem.Services
{
    public class AvailabilityService
    {
        private readonly AppDbContext _context;

        public AvailabilityService(AppDbContext context)
        {
            _context = context;
        }

        public List<TimeSpan> GetAvailableSlots(int doctorId, DateTime date)
        {
            var availability = _context.DoctorAvailabilities
                .FirstOrDefault(x => x.DoctorId == doctorId && x.Date.Date == date.Date);

            if (availability == null)
                throw new Exception("No availability found");

            var allSlots = SlotGeneratorHelper.GenerateSlots(
                availability.StartTime,
                availability.EndTime,
                availability.SlotDurationMinutes
            );

            var bookedSlots = _context.Appointments
                .Where(a => a.DoctorId == doctorId && a.AppointmentDate.Date == date.Date)
                .Select(a => a.SlotTime)
                .ToList();

            return allSlots.Where(s => !bookedSlots.Contains(s)).ToList();
        }
    }
}