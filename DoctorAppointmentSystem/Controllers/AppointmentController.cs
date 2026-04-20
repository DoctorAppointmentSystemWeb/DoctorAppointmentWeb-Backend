using Microsoft.AspNetCore.Mvc;
using DoctorAppointmentSystem.DTOs;
using DoctorAppointmentSystem.Enum;
using DoctorAppointmentSystem.Services;

namespace DoctorAppointmentSystem.Controllers
{
    [ApiController]
    [Route("api/appointment")]
    public class AppointmentController : ControllerBase
    {
        private readonly AppointmentService _service;

        public AppointmentController(AppointmentService service)
        {
            _service = service;
        }

        [HttpPost("book")]
        public IActionResult Book(AppointmentDto dto)
        {
            var result = _service.BookAppointment(dto);
            return Ok(result);
        }

        [HttpPut("status/{id}")]
        public IActionResult UpdateStatus(int id, AppointmentStatus status)
        {
            _service.UpdateStatus(id, status);
            return Ok("Updated Successfully");
        }
    }
}