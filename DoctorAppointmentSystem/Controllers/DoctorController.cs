using DoctorAppointmentSystem.DTOs;
using DoctorAppointmentSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoctorAppointmentSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // ?? All endpoints require login
    public class DoctorController : ControllerBase
    {
        private readonly DoctorService _service;

        public DoctorController(DoctorService service)
        {
            _service = service;
        }

        // ?? Any logged-in user can view doctors
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _service.GetAll());
        }

        // ?? Only Admin can add doctors
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(DoctorDto dto)
        {
            return Ok(await _service.Create(dto));
        }
    }
}