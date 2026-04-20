using DoctorAppointmentSystem.DTOs;
using DoctorAppointmentSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoctorAppointmentSystem.Controllers
{
    [ApiController]
    [Route("api/reminders")]
    [Authorize(Roles = "Admin")]
    public class ReminderController : ControllerBase
    {
        private readonly ReminderService _service;

        public ReminderController(ReminderService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReminderCreateRequest dto)
        {
            try
            {
                var result = await _service.CreateReminderAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPending()
        {
            try
            {
                var result = await _service.GetAllPendingRemindersAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("appointment/{appointmentId}")]
        public async Task<IActionResult> GetByAppointment(int appointmentId)
        {
            try
            {
                var result = await _service.GetRemindersByAppointmentAsync(appointmentId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("{reminderId}/send")]
        public async Task<IActionResult> Send(int reminderId)
        {
            try
            {
                var result = await _service.SendReminderAsync(reminderId);
                return Ok(new { success = result, message = result ? "Reminder sent successfully" : "Reminder sending failed" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("send-due")]
        public async Task<IActionResult> SendDue()
        {
            try
            {
                var count = await _service.SendDueRemindersAsync();
                return Ok(new { message = $"Due reminders processed successfully. Sent count: {count}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}