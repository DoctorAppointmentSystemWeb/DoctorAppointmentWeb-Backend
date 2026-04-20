using DoctorAppointmentSystem.Data;
using DoctorAppointmentSystem.DTOs;
using DoctorAppointmentSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace DoctorAppointmentSystem.Services
{
    public class SpecialtyService
    {
        private readonly AppDbContext _context;

        public SpecialtyService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<SpecialtyDto>> GetAll()
        {
            return await _context.Specialties
                .Select(x => new SpecialtyDto
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync();
        }

        public async Task<SpecialtyDto> Create(SpecialtyDto dto)
        {
            var entity = new Specialty
            {
                Name = dto.Name
            };

            _context.Specialties.Add(entity);
            await _context.SaveChangesAsync();

            dto.Id = entity.Id;
            return dto;
        }
    }
}