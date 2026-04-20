using DoctorAppointmentSystem.Data;
using DoctorAppointmentSystem.DTOs;
using DoctorAppointmentSystem.Enum;
using DoctorAppointmentSystem.Helpers;
using Microsoft.EntityFrameworkCore;

namespace DoctorAppointmentSystem.Services
{
    public class DashboardService
    {
        private readonly AppDbContext _context;

        public DashboardService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardOverviewResponse> GetOverviewAsync(DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                var query = _context.Appointments.AsQueryable();

                if (fromDate.HasValue)
                    query = query.Where(a => a.AppointmentDate.Date >= fromDate.Value.Date);

                if (toDate.HasValue)
                    query = query.Where(a => a.AppointmentDate.Date <= toDate.Value.Date);

                var appointments = await query.ToListAsync();

                var response = new DashboardOverviewResponse
                {
                    TotalAppointments = appointments.Count,
                    ConfirmedAppointments = appointments.Count(a => a.Status == AppointmentStatus.Confirmed),
                    CompletedAppointments = appointments.Count(a => a.Status == AppointmentStatus.Completed),
                    CancelledAppointments = appointments.Count(a => a.Status == AppointmentStatus.Cancelled),
                    NoShowAppointments = appointments.Count(a => a.Status == AppointmentStatus.NoShow),
                    TotalRevenue = RevenueCalculatorHelper.CalculateRevenue(appointments),
                    TotalDoctors = await _context.Doctors.CountAsync(),
                    TotalPatients = await _context.Users.CountAsync(u => u.Role == "User")
                };

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DashboardService] GetOverviewAsync Error: {ex.Message}");
                throw;
            }
        }

        public async Task<List<RevenueByModeResponse>> GetRevenueByModeAsync(DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                var query = _context.Appointments.AsQueryable();

                if (fromDate.HasValue)
                    query = query.Where(a => a.AppointmentDate.Date >= fromDate.Value.Date);

                if (toDate.HasValue)
                    query = query.Where(a => a.AppointmentDate.Date <= toDate.Value.Date);

                var appointments = await query.ToListAsync();

                return appointments
                    .GroupBy(a => a.Mode)
                    .Select(g => new RevenueByModeResponse
                    {
                        Mode = g.Key,
                        Revenue = RevenueCalculatorHelper.CalculateRevenue(g),
                        AppointmentsCount = g.Count()
                    })
                    .OrderBy(x => x.Mode)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DashboardService] GetRevenueByModeAsync Error: {ex.Message}");
                throw;
            }
        }

        public async Task<List<RevenueBySpecialtyResponse>> GetRevenueBySpecialtyAsync(DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                var query = _context.Appointments
                    .Include(a => a.Doctor)
                    .ThenInclude(d => d.Specialty)
                    .AsQueryable();

                if (fromDate.HasValue)
                    query = query.Where(a => a.AppointmentDate.Date >= fromDate.Value.Date);

                if (toDate.HasValue)
                    query = query.Where(a => a.AppointmentDate.Date <= toDate.Value.Date);

                var appointments = await query.ToListAsync();

                return appointments
                    .Where(a => a.Doctor != null && a.Doctor.Specialty != null)
                    .GroupBy(a => new
                    {
                        SpecialtyId = a.Doctor.SpecialtyId,
                        SpecialtyName = a.Doctor.Specialty.Name
                    })
                    .Select(g => new RevenueBySpecialtyResponse
                    {
                        SpecialtyId = g.Key.SpecialtyId,
                        SpecialtyName = g.Key.SpecialtyName,
                        Revenue = RevenueCalculatorHelper.CalculateRevenue(g),
                        AppointmentsCount = g.Count()
                    })
                    .OrderBy(x => x.SpecialtyName)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DashboardService] GetRevenueBySpecialtyAsync Error: {ex.Message}");
                throw;
            }
        }

        public async Task<List<AppointmentTrendResponse>> GetAppointmentTrendsAsync(DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                var query = _context.Appointments.AsQueryable();

                if (fromDate.HasValue)
                    query = query.Where(a => a.AppointmentDate.Date >= fromDate.Value.Date);

                if (toDate.HasValue)
                    query = query.Where(a => a.AppointmentDate.Date <= toDate.Value.Date);

                var appointments = await query.ToListAsync();

                return appointments
                    .GroupBy(a => a.AppointmentDate.Date)
                    .Select(g => new AppointmentTrendResponse
                    {
                        Date = g.Key,
                        Count = g.Count(),
                        Revenue = RevenueCalculatorHelper.CalculateRevenue(g)
                    })
                    .OrderBy(x => x.Date)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DashboardService] GetAppointmentTrendsAsync Error: {ex.Message}");
                throw;
            }
        }

        public async Task<List<DoctorUtilizationResponse>> GetDoctorUtilizationAsync(DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                var query = _context.Appointments.AsQueryable();

                if (fromDate.HasValue)
                    query = query.Where(a => a.AppointmentDate.Date >= fromDate.Value.Date);

                if (toDate.HasValue)
                    query = query.Where(a => a.AppointmentDate.Date <= toDate.Value.Date);

                var appointments = await query.ToListAsync();
                var doctors = await _context.Doctors.ToListAsync();

                return appointments
                    .GroupBy(a => a.DoctorId)
                    .Select(g =>
                    {
                        var doctor = doctors.FirstOrDefault(d => d.Id == g.Key);

                        return new DoctorUtilizationResponse
                        {
                            DoctorId = g.Key,
                            DoctorName = doctor != null ? doctor.Name : "Unknown",
                            TotalAppointments = g.Count(),
                            CompletedAppointments = g.Count(x => x.Status == AppointmentStatus.Completed),
                            Revenue = RevenueCalculatorHelper.CalculateRevenue(g)
                        };
                    })
                    .OrderByDescending(x => x.TotalAppointments)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DashboardService] GetDoctorUtilizationAsync Error: {ex.Message}");
                throw;
            }
        }
    }
}