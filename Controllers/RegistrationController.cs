using advent_appointment_booking.Models;
using advent_appointment_booking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using log4net;

namespace advent_appointment_booking.Controllers
{
    [Route("api/[controller]")]

    
    [ApiController]
    [AllowAnonymous]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;
        private readonly ILog _logger;

        public RegistrationController(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
            _logger = LogManager.GetLogger(typeof(CustomExceptionFilter));
        }

        [HttpPost("truckingcompany")]
        public async Task<IActionResult> RegisterTruckingCompany([FromBody] TruckingCompany model)
        {
            _logger.Info(DateTime.Today.ToLongDateString()+": RegisterTruckingCompany process started " + model);
            // Validate incoming model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _registrationService.RegisterTruckingCompnay(model);
                _logger.Info(DateTime.Today.ToLongDateString()+": Trucking company resigtered Successfully  ");

                return Ok(new { message = "Trucking company registered successfully." });
            }
            catch (Exception ex)
            {
                _logger.Error(DateTime.Today.ToLongDateString()+": Registering Trucking Company Failed " + ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("terminal")]
        public async Task<IActionResult> RegisterTerminal([FromBody] Terminal model)
        {
            _logger.Info(DateTime.Today.ToLongDateString()+": RegisterTerminal process started with Model" + model);
            // Validate incoming model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _registrationService.RegisterTerminal(model);
                _logger.Info(DateTime.Today.ToLongDateString()+": Terminal registered  Successfully with model" +  model);
                return Ok(new { message = "Terminal registered successfully." });
            }
            catch (Exception ex)
            {
                _logger.Error(DateTime.Today.ToLongDateString()+": Failed to register terminal with model." + model + ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
