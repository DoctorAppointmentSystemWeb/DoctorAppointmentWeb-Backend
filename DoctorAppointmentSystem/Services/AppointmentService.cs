using System;
using System.Linq;
using DoctorAppointmentSystem.Data;
using DoctorAppointmentSystem.DTOs;
using DoctorAppointmentSystem.Enum;
using DoctorAppointmentSystem.Helpers;
using DoctorAppointmentSystem.Models;

namespace DoctorAppointmentSystem.Services
{
    public class AppointmentService
    {
        private readonly AppDbContext _context;

        public AppointmentService(AppDbContext context)
        {
            _context = context;
        }

        public string BookAppointment(AppointmentDto dto)
        {
            // ?? Prevent double booking
            var exists = _context.Appointments.Any(a =>
                a.DoctorId == dto.DoctorId &&
                a.AppointmentDate.Date == dto.Date.Date &&
                a.SlotTime == dto.SlotTime);

            if (exists)
                throw new Exception("Slot already booked");

            // ?? Online & Offline must use different doctors
            var sameUserSameDay = _context.Appointments
                .Where(a => a.UserId == dto.UserId && a.AppointmentDate.Date == dto.Date.Date)
                .ToList();

            foreach (var appt in sameUserSameDay)
            {
                if (appt.Mode != dto.Mode && appt.DoctorId == dto.DoctorId)
                    throw new Exception("Online & Offline must use different doctors");
            }

            var appointment = new Appointment
            {
                DoctorId = dto.DoctorId,
                UserId = dto.UserId,
                AppointmentDate = dto.Date,
                SlotTime = dto.SlotTime,
                Mode = dto.Mode,
                Status = AppointmentStatus.Confirmed
            };

            _context.Appointments.Add(appointment);
            _context.SaveChanges();

            AuditHelper.Log($"Appointment booked for User {dto.UserId}");

            return "Appointment Confirmed";
        }

        public void UpdateStatus(int id, AppointmentStatus status)
        {
            var appt = _context.Appointments.Find(id);

            if (appt == null)
                throw new Exception("Appointment not found");

            appt.Status = status;
            _context.SaveChanges();

            AuditHelper.Log($"Appointment {id} updated to {status}");
        }
    }
}