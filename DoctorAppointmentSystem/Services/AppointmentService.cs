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
            var exists = _context.Appointments.Any(a =>
                a.DoctorId == dto.DoctorId &&
                a.AppointmentDate.Date == dto.Date.Date &&
                a.SlotTime == dto.SlotTime);

            if (exists)
                throw new Exception("Slot already booked");

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

        public void UpdateStatusWithAuthorization(int id, AppointmentStatus status, int userId, string role)
        {
            var appt = _context.Appointments.FirstOrDefault(a => a.Id == id);

            if (appt == null)
                throw new Exception("Appointment not found");

            if (role == "User")
            {
                if (appt.UserId != userId)
                    throw new Exception("Unauthorized access");

                if (status != AppointmentStatus.Cancelled)
                    throw new Exception("Users can only cancel appointments");
            }
            else if (role == "Doctor")
            {
                if (appt.DoctorId != userId)
                    throw new Exception("Unauthorized access");

                if (status != AppointmentStatus.Completed &&
                    status != AppointmentStatus.NoShow)
                    throw new Exception("Doctor can only mark Completed or NoShow");
            }
            else if (role == "Admin")
            {
            }
            else
            {
                throw new Exception("Invalid role");
            }

            appt.Status = status;
            _context.SaveChanges();

            AuditHelper.Log($"Appointment {id} updated to {status} by {role}");
        }

        public object GetAppointmentsByUser(int userId)
        {
            return _context.Appointments
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.AppointmentDate)
                .ToList();
        }

        public object GetAppointmentsByDoctor(int doctorId)
        {
            return _context.Appointments
                .Where(a => a.DoctorId == doctorId)
                .OrderByDescending(a => a.AppointmentDate)
                .ToList();
        }

        public object GetAllAppointments()
        {
            return _context.Appointments
                .OrderByDescending(a => a.AppointmentDate)
                .ToList();
        }
    }
}