using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DoctorAppointmentSystem.DTOs;
using DoctorAppointmentSystem.Enum;
using DoctorAppointmentSystem.Services;

namespace DoctorAppointmentSystem.Controllers
{
    [ApiController]
    [Route("api/appointment")]
    [Authorize] 
    public class AppointmentController : ControllerBase
    {
        private readonly AppointmentService _service;

        public AppointmentController(AppointmentService service)
        {
            _service = service;
        }

        
        [HttpPost("book")]
        [Authorize(Roles = "User")]
        public IActionResult Book(AppointmentDto dto)
        {
            
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            
            if (userId != dto.UserId)
                return Unauthorized("You can only book for yourself");

            var result = _service.BookAppointment(dto);
            return Ok(result);
        }

        
        [HttpPut("status/{id}")]
        public IActionResult UpdateStatus(int id, AppointmentStatus status)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var role = User.FindFirst(ClaimTypes.Role).Value;

            _service.UpdateStatusWithAuthorization(id, status, userId, role);

            return Ok("Updated Successfully");
        }
    }
}