using DoctorAppointmentSystem.Enum;
using DoctorAppointmentSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace DoctorAppointmentSystem.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAdminAsync(AppDbContext context)
        {
            try
            {
                Console.WriteLine("[DbInitializer] Checking for admin...");

                var adminExists = await context.Users
                    .AnyAsync(u => u.Role == UserRole.Admin.ToString());

                if (adminExists)
                {
                    Console.WriteLine("[DbInitializer] Admin already exists.");
                    return;
                }

                var admin = new User
                {
                    Name = "Admin",
                    Email = "admin@gmail.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    Role = UserRole.Admin.ToString(),
                    CreatedAt = DateTime.UtcNow
                };

                await context.Users.AddAsync(admin);
                await context.SaveChangesAsync();

                Console.WriteLine("[DbInitializer] Admin created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DbInitializer][Error] {ex.Message}");
            }
        }
    }
}
