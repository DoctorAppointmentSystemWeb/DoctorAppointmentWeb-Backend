using DoctorAppointmentSystem.Data;
using DoctorAppointmentSystem.DTOs;
using DoctorAppointmentSystem.Enum;
using DoctorAppointmentSystem.Helpers;
using DoctorAppointmentSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace DoctorAppointmentSystem.Services
{
    public class ReminderService
    {
        private readonly AppDbContext _context;
        private readonly EmailService _emailService;

        public ReminderService(AppDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<ReminderResponse> CreateReminderAsync(ReminderCreateRequest dto)
        {
            try
            {
                Console.WriteLine($"[ReminderService] Creating reminder for AppointmentId: {dto.AppointmentId}");

                var appointment = await _context.Appointments
                    .FirstOrDefaultAsync(a => a.Id == dto.AppointmentId);

                if (appointment == null)
                    throw new Exception("Appointment not found.");

                if (appointment.Status == AppointmentStatus.Cancelled)
                    throw new Exception("Cannot create reminder for cancelled appointment.");

                var patient = await _context.Users.FirstOrDefaultAsync(u => u.Id == appointment.UserId);
                var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Id == appointment.DoctorId);

                var patientName = patient?.Name ?? "Patient";
                var doctorName = doctor?.Name ?? "Doctor";

                var subject = EmailTemplateHelper.BuildReminderSubject(appointment);
                var message = EmailTemplateHelper.BuildReminderBody(appointment, patientName, doctorName);

                var reminder = new AppointmentReminder
                {
                    AppointmentId = dto.AppointmentId,
                    ReminderDateTime = dto.ReminderDateTime,
                    ReminderType = dto.ReminderType,
                    EmailTo = patient?.Email,
                    Subject = subject,
                    Message = message,
                    IsSent = false,
                    CreatedAt = DateTime.UtcNow
                };

                _context.AppointmentReminders.Add(reminder);
                await _context.SaveChangesAsync();

                return new ReminderResponse
                {
                    Id = reminder.Id,
                    AppointmentId = reminder.AppointmentId,
                    ReminderDateTime = reminder.ReminderDateTime,
                    ReminderType = reminder.ReminderType,
                    IsSent = reminder.IsSent,
                    SentAt = reminder.SentAt,
                    EmailTo = reminder.EmailTo,
                    Subject = reminder.Subject,
                    Message = reminder.Message,
                    ErrorMessage = reminder.ErrorMessage,
                    CreatedAt = reminder.CreatedAt
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ReminderService] CreateReminderAsync Error: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> SendReminderAsync(int reminderId)
        {
            try
            {
                var reminder = await _context.AppointmentReminders
                    .FirstOrDefaultAsync(r => r.Id == reminderId);

                if (reminder == null)
                    throw new Exception("Reminder not found.");

                if (reminder.IsSent)
                    throw new Exception("Reminder already sent.");

                if (string.IsNullOrWhiteSpace(reminder.EmailTo))
                    throw new Exception("Recipient email is missing.");

                var sent = await _emailService.SendEmailAsync(
                    reminder.EmailTo,
                    reminder.Subject ?? "Appointment Reminder",
                    reminder.Message ?? ""
                );

                if (!sent)
                {
                    reminder.ErrorMessage = "Email sending failed.";
                    await _context.SaveChangesAsync();
                    return false;
                }

                reminder.IsSent = true;
                reminder.SentAt = DateTime.UtcNow;
                reminder.ErrorMessage = null;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ReminderService] SendReminderAsync Error: {ex.Message}");
                throw;
            }
        }

        public async Task<List<ReminderResponse>> GetAllPendingRemindersAsync()
        {
            var reminders = await _context.AppointmentReminders
                .Where(r => !r.IsSent)
                .OrderBy(r => r.ReminderDateTime)
                .ToListAsync();

            return reminders.Select(r => new ReminderResponse
            {
                Id = r.Id,
                AppointmentId = r.AppointmentId,
                ReminderDateTime = r.ReminderDateTime,
                ReminderType = r.ReminderType,
                IsSent = r.IsSent,
                SentAt = r.SentAt,
                EmailTo = r.EmailTo,
                Subject = r.Subject,
                Message = r.Message,
                ErrorMessage = r.ErrorMessage,
                CreatedAt = r.CreatedAt
            }).ToList();
        }

        public async Task<List<ReminderResponse>> GetRemindersByAppointmentAsync(int appointmentId)
        {
            var reminders = await _context.AppointmentReminders
                .Where(r => r.AppointmentId == appointmentId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return reminders.Select(r => new ReminderResponse
            {
                Id = r.Id,
                AppointmentId = r.AppointmentId,
                ReminderDateTime = r.ReminderDateTime,
                ReminderType = r.ReminderType,
                IsSent = r.IsSent,
                SentAt = r.SentAt,
                EmailTo = r.EmailTo,
                Subject = r.Subject,
                Message = r.Message,
                ErrorMessage = r.ErrorMessage,
                CreatedAt = r.CreatedAt
            }).ToList();
        }

        public async Task<int> SendDueRemindersAsync()
        {
            try
            {
                Console.WriteLine("[ReminderService] Processing due reminders...");

                var dueReminders = await _context.AppointmentReminders
                    .Where(r => !r.IsSent && r.ReminderDateTime <= DateTime.UtcNow)
                    .ToListAsync();

                if (!dueReminders.Any())
                {
                    Console.WriteLine("[ReminderService] No due reminders found.");
                    return 0;
                }

                int sentCount = 0;

                foreach (var reminder in dueReminders)
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(reminder.EmailTo))
                        {
                            reminder.ErrorMessage = "Recipient email is missing.";
                            continue;
                        }

                        var sent = await _emailService.SendEmailAsync(
                            reminder.EmailTo,
                            reminder.Subject ?? "Appointment Reminder",
                            reminder.Message ?? ""
                        );

                        if (sent)
                        {
                            reminder.IsSent = true;
                            reminder.SentAt = DateTime.UtcNow;
                            reminder.ErrorMessage = null;
                            sentCount++;
                        }
                        else
                        {
                            reminder.ErrorMessage = "Email sending failed.";
                        }
                    }
                    catch (Exception ex)
                    {
                        reminder.ErrorMessage = ex.Message;
                        Console.WriteLine($"[ReminderService] Error while sending reminder Id {reminder.Id}: {ex.Message}");
                    }
                }

                await _context.SaveChangesAsync();

                Console.WriteLine($"[ReminderService] Due reminders processed. Sent count: {sentCount}");
                return sentCount;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ReminderService] SendDueRemindersAsync Error: {ex.Message}");
                throw;
            }
        }
    }
}