using AppointmentBooking.DTOs;
using AppointmentBooking.Services;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentBooking.Controllers
{
    [Route("calendar/query")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly AppointmentService _appointmentService;
        private readonly ILogger<AppointmentController> _logger;

        public AppointmentController(AppointmentService appointmentService, ILogger<AppointmentController> logger)
        {
            _appointmentService = appointmentService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> GetAvailableSlots([FromBody] SearchCriteria request)
        {
            try
            {
                _logger.LogInformation("Received request for available slots. Date: {Date}, Products: {Products}, Language: {Language}, Rating: {Rating}",
             request.Date, string.Join(",", request.Products), request.Language, request.Rating);

                var availableSlots = await _appointmentService.GetAvailableSlots(request);

                return Ok(availableSlots);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Validation error: {Message}", ex.Message);
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching available slots: {Message}", ex.Message);
                return StatusCode(500, new { error = "Internal Server Error. Please try again later." });
            }
        }
    }
}
