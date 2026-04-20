using DoctorAppointmentSystem.Data;
using DoctorAppointmentSystem.DTOs;
using DoctorAppointmentSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace DoctorAppointmentSystem.Services
{
    public class DoctorService
    {
        private readonly AppDbContext _context;

        public DoctorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<DoctorDto>> GetAll()
        {
            return await _context.Doctors
                .Select(x => new DoctorDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Qualification = x.Qualification,
                    ExperienceYears = x.ExperienceYears,
                    Mode = x.Mode,
                    SpecialtyId = x.SpecialtyId
                }).ToListAsync();
        }

        public async Task<DoctorDto> Create(DoctorDto dto)
        {
            var entity = new Doctor
            {
                Name = dto.Name,
                Qualification = dto.Qualification,
                ExperienceYears = dto.ExperienceYears,
                Mode = dto.Mode,
                SpecialtyId = dto.SpecialtyId
            };

            _context.Doctors.Add(entity);
            await _context.SaveChangesAsync();

            dto.Id = entity.Id;
            return dto;
        }
    }
}