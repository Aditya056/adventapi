using advent_appointment_booking.DTOs;
using advent_appointment_booking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using log4net;
using System.ComponentModel.DataAnnotations;

namespace advent_appointment_booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
         private readonly ILog _logger;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
            _logger = LogManager.GetLogger(typeof(CustomExceptionFilter));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login login, [FromQuery] string userType)
        {
            _logger.Info(DateTime.Today.ToLongDateString()+" : Login process started for userType =" +  userType);

            if (!ModelState.IsValid)
            {
                _logger.Error(DateTime.Today.ToLongDateString()+" : Login attempt Failed for UserType = "  + userType);
                return BadRequest(ModelState);
                
            }
            
            try
            {
                var result = await _authService.LoginAsync(login.Email, login.Password, userType);
                 _logger.Info(DateTime.Today.ToLongDateString()+": LoggedIn successfully with " + login.Email + " " +  userType);

                return Ok(result); 
            }
            catch (Exception ex)
            {
                _logger.Error(DateTime.Today.ToLongDateString()+" : Failed to Login as  " +  userType + " " + ex.Message);

                return Unauthorized(new { message = ex.Message });
            }
        }
    }
}
