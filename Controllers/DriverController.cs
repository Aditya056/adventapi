using advent_appointment_booking.Enums;
using advent_appointment_booking.Models;
using advent_appointment_booking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using advent_appointment_booking.DTOs;
using log4net;

namespace advent_appointment_booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly IDriverService _driverService;
        private readonly ILog _logger;

        public DriverController(IDriverService driverService)
        {
            _driverService = driverService;
            _logger = LogManager.GetLogger(typeof(CustomExceptionFilter));
        }

        // Create a new driver
        [HttpPost]
        [Authorize(Policy = Policy.RequireTruckingCompanyRole)]
        public async Task<IActionResult> CreateDriver([FromBody] Driver driver)
        {
            _logger.Info(DateTime.Today.ToLongDateString()+": CreateDriver process started...");
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                _logger.Error(DateTime.Today.ToLongDateString()+": Validation failed for " + driver + " " + errors);
                return BadRequest(new { message = "Validation failed", errors });
            }

            try
            {
                var result = await _driverService.CreateDriver(driver);
                _logger.Info(DateTime.Today.ToLongDateString()+": Driver created successfully ");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.Error(DateTime.Today.ToLongDateString()+": Failed to create driver" + ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        // Get details of a driver by ID
        [HttpGet("{driverId}")]
        public async Task<IActionResult> GetDriver(int driverId)
        {
            _logger.Info(DateTime.Today.ToLongDateString()+": Attempted to fetch driver details using DriverId "  + driverId);

            try {
                var driver = await _driverService.GetDriver(driverId);
                _logger.Info(DateTime.Today.ToLongDateString()+": Driver details fetched successfully using DriverId " + driverId );
                return Ok(driver);
            }
            catch(Exception ex) { 
                _logger.Error(DateTime.Today.ToLongDateString()+": Failed to fetch driver details using DriverId" +  driverId + " "+ ex.Message);
                return NotFound(new { message = ex.Message });
            }
        }


        // Get all drivers
        [HttpGet]
        [Authorize(Policy = Policy.RequireTruckingCompanyRole)]
        public async Task<IActionResult> GetAllDrivers()
        {
            var drivers = await _driverService.GetAllDrivers();
            _logger.Info(DateTime.Today.ToLongDateString()+" :  Drivers details fetched successfully" );
            return Ok(drivers);
        }


        // Update a driver
        [HttpPut("{driverId}")]
        [Authorize(Policy = Policy.RequireTruckingCompanyRole)]
        public async Task<IActionResult> UpdateDriver(int driverId, [FromBody] Driver driver)
        {
             _logger.Info(DateTime.Today.ToLongDateString()+" :  UpdateDriver process  initiated using DriverId " + driverId );

             
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                _logger.Error(DateTime.Today.ToLongDateString()+" : Validation failed for  " + driverId + " " + errors);
                return BadRequest(new { message = "Validation failed", errors });
                

            }

            try
            {
                var result = await _driverService.UpdateDriver(driverId, driver);
                _logger.Info(DateTime.Today.ToLongDateString()+" : Driver details updated successfully for Id " + driverId) ;
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                _logger.Error(DateTime.Today.ToLongDateString()+" : Failed to update driver details +  " + ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        // Delete a driver
        [HttpDelete("{driverId}")]
        [Authorize(Policy = Policy.RequireTruckingCompanyRole)]
        public async Task<IActionResult> DeleteDriver(int driverId)
        {
            _logger.Info(DateTime.Today.ToLongDateString()+" : Deletedriver  process initiated using DriverId " + driverId );

            try
            {
                var result = await _driverService.DeleteDriver(driverId);
                _logger.Info(DateTime.Today.ToLongDateString()+" : Driver deleted successfully  with  Id " + driverId) ;

                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                _logger.Error(DateTime.Today.ToLongDateString()+" : Failed to delete driver with Id " + driverId+ " " + ex.Message);
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
