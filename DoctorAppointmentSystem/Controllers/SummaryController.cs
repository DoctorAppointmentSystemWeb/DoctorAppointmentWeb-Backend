using DoctorAppointmentSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoctorAppointmentSystem.Controllers
{
    [ApiController]
    [Route("api/summaries")]
    [Authorize(Roles = "Admin")]
    public class SummaryController : ControllerBase
    {
        private readonly SummaryService _service;

        public SummaryController(SummaryService service)
        {
            _service = service;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> Generate([FromQuery] DateTime date)
        {
            try
            {
                var result = await _service.GenerateDailySummaryAsync(date);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("daily")]
        public async Task<IActionResult> GetDaily([FromQuery] DateTime date)
        {
            try
            {
                var result = await _service.GetDailySummaryByDateAsync(date);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("mode")]
        public async Task<IActionResult> GetByMode([FromQuery] DateTime date)
        {
            try
            {
                var result = await _service.GetSummaryByModeAsync(date);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("specialty")]
        public async Task<IActionResult> GetBySpecialty([FromQuery] DateTime date)
        {
            try
            {
                var result = await _service.GetSummaryBySpecialtyAsync(date);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}