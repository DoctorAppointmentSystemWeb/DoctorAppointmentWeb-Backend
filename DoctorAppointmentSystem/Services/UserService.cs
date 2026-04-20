using DoctorAppointmentSystem.Data;
using DoctorAppointmentSystem.DTOs;
using DoctorAppointmentSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace DoctorAppointmentSystem.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            try
            {
                Console.WriteLine("[UserService] Fetching all users");

                var users = await _context.Users
                    .OrderByDescending(u => u.CreatedAt)
                    .Select(u => new UserDto
                    {
                        UserId = u.Id,
                        Name = u.Name,
                        Email = u.Email,
                        Role = u.Role,
                        CreatedAt = u.CreatedAt
                    })
                    .ToListAsync();

                return users;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UserService] GetAllUsersAsync Error: {ex.Message}");
                throw;
            }
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            try
            {
                Console.WriteLine($"[UserService] Fetching user by Id: {id}");

                var user = await _context.Users
                    .Where(u => u.Id == id)
                    .Select(u => new UserDto
                    {
                        UserId = u.Id,
                        Name = u.Name,
                        Email = u.Email,
                        Role = u.Role,
                        CreatedAt = u.CreatedAt
                    })
                    .FirstOrDefaultAsync();

                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UserService] GetUserByIdAsync Error: {ex.Message}");
                throw;
            }
        }

        public async Task<UserDto> UpdateUserAsync(int id, UpdateUserDto dto)
        {
            try
            {
                Console.WriteLine($"[UserService] Updating user Id: {id}");

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

                if (user == null)
                    throw new Exception("User not found.");

                if (string.IsNullOrWhiteSpace(dto.Name))
                    throw new Exception("Name cannot be empty.");

                user.Name = dto.Name.Trim();

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return new UserDto
                {
                    UserId = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role,
                    CreatedAt = user.CreatedAt
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UserService] UpdateUserAsync Error: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            try
            {
                Console.WriteLine($"[UserService] Deleting user Id: {id}");

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

                if (user == null)
                    throw new Exception("User not found.");

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UserService] DeleteUserAsync Error: {ex.Message}");
                throw;
            }
        }

        public async Task<UserDto> UpdateUserRoleAsync(int id, string role)
        {
            try
            {
                Console.WriteLine($"[UserService] Updating role for user Id: {id}");

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

                if (user == null)
                    throw new Exception("User not found.");

                if (string.IsNullOrWhiteSpace(role))
                    throw new Exception("Role cannot be empty.");

                role = role.Trim();

                if (role != "Admin" && role != "User")
                    throw new Exception("Invalid role. Allowed roles are Admin or User.");

                user.Role = role;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return new UserDto
                {
                    UserId = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role,
                    CreatedAt = user.CreatedAt
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UserService] UpdateUserRoleAsync Error: {ex.Message}");
                throw;
            }
        }
    }
}