using DoctorAppointmentSystem.DTOs;
using DoctorAppointmentSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoctorAppointmentSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class SpecialtyController : ControllerBase
    {
        private readonly SpecialtyService _service;

        public SpecialtyController(SpecialtyService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _service.GetAll());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(SpecialtyDto dto)
        {
            return Ok(await _service.Create(dto));
        }
    }
}