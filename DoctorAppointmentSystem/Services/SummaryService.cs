using DoctorAppointmentSystem.Data;
using DoctorAppointmentSystem.DTOs;
using DoctorAppointmentSystem.Enum;
using DoctorAppointmentSystem.Helpers;
using DoctorAppointmentSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace DoctorAppointmentSystem.Services
{
    public class SummaryService
    {
        private readonly AppDbContext _context;

        public SummaryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<DailySummaryResponse>> GenerateDailySummaryAsync(DateTime date)
        {
            try
            {
                Console.WriteLine($"[SummaryService] Generating summary for {date:yyyy-MM-dd}");

                var targetDate = date.Date;

                var appointments = await _context.Appointments
                    .Where(a => a.AppointmentDate.Date == targetDate)
                    .ToListAsync();

                if (!appointments.Any())
                {
                    return new List<DailySummaryResponse>();
                }

                var existingSummaries = await _context.DailySummaries
                    .Where(s => s.SummaryDate.Date == targetDate)
                    .ToListAsync();

                if (existingSummaries.Any())
                {
                    _context.DailySummaries.RemoveRange(existingSummaries);
                    await _context.SaveChangesAsync();
                }

                var summaries = appointments
                    .GroupBy(a => a.Mode)
                    .Select(g => new DailySummary
                    {
                        SummaryDate = targetDate,
                        Mode = g.Key,
                        SpecialtyId = null,
                        SpecialtyName = null,
                        TotalAppointments = g.Count(),
                        ConfirmedAppointments = g.Count(x => x.Status == AppointmentStatus.Confirmed),
                        CompletedAppointments = g.Count(x => x.Status == AppointmentStatus.Completed),
                        CancelledAppointments = g.Count(x => x.Status == AppointmentStatus.Cancelled),
                        NoShowAppointments = g.Count(x => x.Status == AppointmentStatus.NoShow),
                        Revenue = RevenueCalculatorHelper.CalculateRevenue(g),
                        GeneratedAt = DateTime.UtcNow
                    })
                    .ToList();

                _context.DailySummaries.AddRange(summaries);
                await _context.SaveChangesAsync();

                return summaries.Select(s => new DailySummaryResponse
                {
                    SummaryDate = s.SummaryDate,
                    Mode = s.Mode,
                    SpecialtyId = s.SpecialtyId,
                    SpecialtyName = s.SpecialtyName,
                    TotalAppointments = s.TotalAppointments,
                    ConfirmedAppointments = s.ConfirmedAppointments,
                    CompletedAppointments = s.CompletedAppointments,
                    CancelledAppointments = s.CancelledAppointments,
                    NoShowAppointments = s.NoShowAppointments,
                    Revenue = s.Revenue
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SummaryService] GenerateDailySummaryAsync Error: {ex.Message}");
                throw;
            }
        }

        public async Task<List<DailySummaryResponse>> GetDailySummaryByDateAsync(DateTime date)
        {
            try
            {
                var targetDate = date.Date;

                var summaries = await _context.DailySummaries
                    .Where(s => s.SummaryDate.Date == targetDate)
                    .OrderBy(s => s.Mode)
                    .ToListAsync();

                return summaries.Select(s => new DailySummaryResponse
                {
                    SummaryDate = s.SummaryDate,
                    Mode = s.Mode,
                    SpecialtyId = s.SpecialtyId,
                    SpecialtyName = s.SpecialtyName,
                    TotalAppointments = s.TotalAppointments,
                    ConfirmedAppointments = s.ConfirmedAppointments,
                    CompletedAppointments = s.CompletedAppointments,
                    CancelledAppointments = s.CancelledAppointments,
                    NoShowAppointments = s.NoShowAppointments,
                    Revenue = s.Revenue
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SummaryService] GetDailySummaryByDateAsync Error: {ex.Message}");
                throw;
            }
        }

        public async Task<List<ModeSummaryResponse>> GetSummaryByModeAsync(DateTime date)
        {
            try
            {
                var targetDate = date.Date;

                var appointments = await _context.Appointments
                    .Where(a => a.AppointmentDate.Date == targetDate)
                    .ToListAsync();

                return appointments
                    .GroupBy(a => a.Mode)
                    .Select(g => new ModeSummaryResponse
                    {
                        Mode = g.Key,
                        TotalAppointments = g.Count(),
                        ConfirmedAppointments = g.Count(x => x.Status == AppointmentStatus.Confirmed),
                        CompletedAppointments = g.Count(x => x.Status == AppointmentStatus.Completed),
                        CancelledAppointments = g.Count(x => x.Status == AppointmentStatus.Cancelled),
                        NoShowAppointments = g.Count(x => x.Status == AppointmentStatus.NoShow),
                        Revenue = RevenueCalculatorHelper.CalculateRevenue(g)
                    })
                    .OrderBy(x => x.Mode)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SummaryService] GetSummaryByModeAsync Error: {ex.Message}");
                throw;
            }
        }

        public async Task<List<SpecialtySummaryResponse>> GetSummaryBySpecialtyAsync(DateTime date)
        {
            return new List<SpecialtySummaryResponse>();
        }
    }
}