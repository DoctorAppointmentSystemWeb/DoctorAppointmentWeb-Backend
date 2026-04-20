using System;
using Microsoft.AspNetCore.Mvc;
using DoctorAppointmentSystem.Services;

namespace DoctorAppointmentSystem.Controllers
{
    [ApiController]
    [Route("api/availability")]
    public class AvailabilityController : ControllerBase
    {
        private readonly AvailabilityService _service;

        public AvailabilityController(AvailabilityService service)
        {
            _service = service;
        }

        [HttpGet("{doctorId}/{date}")]
        public IActionResult GetSlots(int doctorId, DateTime date)
        {
            var slots = _service.GetAvailableSlots(doctorId, date);
            return Ok(slots);
        }
    }
}