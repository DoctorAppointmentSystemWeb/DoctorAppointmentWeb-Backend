using DoctorAppointmentSystem.Data;
using DoctorAppointmentSystem.DTOs;
using DoctorAppointmentSystem.Helpers;
using DoctorAppointmentSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace DoctorAppointmentSystem.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly JwtHelper _jwt;
        private readonly PasswordHelper _hasher;
        private readonly EmailService _emailService;

        public AuthService(
            AppDbContext context,
            JwtHelper jwt,
            PasswordHelper hasher,
            EmailService emailService)
        {
            _context = context;
            _jwt = jwt;
            _hasher = hasher;
            _emailService = emailService;
        }

        public async Task<AuthResponseDto> Register(RegisterDto dto)
        {
            try
            {
                Console.WriteLine($"[AuthService] Register started for Email: {dto.Email}");

                if (await _context.Users.AnyAsync(x => x.Email == dto.Email))
                    throw new Exception("User already exists");

                var user = new User
                {
                    Name = dto.Name,
                    Email = dto.Email,
                    PasswordHash = _hasher.Hash(dto.Password),
                    Role = "User"
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                Console.WriteLine("[AuthService] User registered successfully");

                // Send welcome email
                var subject = "Welcome to Doctor Appointment System";
                var body = $@"Hello {user.Name},

Your account has been created successfully.

Email: {user.Email}
Role: {user.Role}

You can now log in and book appointments.

Regards,
Doctor Appointment System Team";

                var emailSent = await _emailService.SendEmailAsync(user.Email, subject, body);

                if (emailSent)
                    Console.WriteLine("[AuthService] Registration email sent successfully");
                else
                    Console.WriteLine("[AuthService] Registration email failed to send");

                return new AuthResponseDto
                {
                    Token = _jwt.GenerateToken(user),
                    Email = user.Email,
                    Role = user.Role
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AuthService] Register Error: {ex.Message}");
                throw;
            }
        }

        public async Task<AuthResponseDto> Login(LoginDto dto)
        {
            try
            {
                Console.WriteLine($"[AuthService] Login started for Email: {dto.Email}");

                var user = await _context.Users
                    .FirstOrDefaultAsync(x => x.Email == dto.Email);

                if (user == null || !_hasher.Verify(dto.Password, user.PasswordHash))
                    throw new Exception("Invalid credentials");

                Console.WriteLine("[AuthService] Login successful");

                // Send login notification email
                var subject = "Login Alert - Doctor Appointment System";
                var body = $@"Hello {user.Name},

Your account was logged in successfully.

Login Time: {DateTime.Now:dd-MM-yyyy hh:mm tt}

If this was not you, please reset your password immediately.

Regards,
Doctor Appointment System Team";

                var emailSent = await _emailService.SendEmailAsync(user.Email, subject, body);

                if (emailSent)
                    Console.WriteLine("[AuthService] Login notification email sent successfully");
                else
                    Console.WriteLine("[AuthService] Login notification email failed to send");

                return new AuthResponseDto
                {
                    Token = _jwt.GenerateToken(user),
                    Email = user.Email,
                    Role = user.Role
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AuthService] Login Error: {ex.Message}");
                throw;
            }
        }
    }
}