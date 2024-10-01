using advent_appointment_booking.DTOs;
using advent_appointment_booking.Enums;
using advent_appointment_booking.Models;
using advent_appointment_booking.Services;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace advent_appointment_booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly ILog _logger;
        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
            _logger = LogManager.GetLogger(typeof(CustomExceptionFilter));
        }

        [HttpPost("create")]
        [Authorize(Policy = Policy.RequireTruckingCompanyRole)]
        public async Task<IActionResult> CreateAppointment([FromBody] Appointment appointment)
        {
            _logger.Info(DateTime.Today.ToLongDateString()+" : Appointment creation process started ...");
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                _logger.Error(DateTime.Today.ToLongDateString() + " : Validation Failed " + errors);
                return BadRequest(new { message = "Validation failed", errors });
            }

            try
            {
                var result = await _appointmentService.CreateAppointment(appointment);
                _logger.Info(DateTime.Today.ToLongDateString()+": Appointment created successfully.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.Error(DateTime.Today.ToLongDateString() + " Appointment creation failed "  + ex.Message);
                return BadRequest(new {message = ex.Message});
            }
        }

        [HttpPut("update/{appointmentId}")]
        [Authorize(Policy = Policy.RequireTruckingCompanyOrTerminalRole)]
        public async Task<IActionResult> UpdateAppointment(int appointmentId, [FromBody] Appointment appointment)
        {
            _logger.Info(DateTime.Today.ToLongDateString()+" : Updating appointment process started for Id   "  + appointmentId);

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                _logger.Error(DateTime.Today.ToLongDateString()+" : Validation failed  " + errors);
                return BadRequest(new { message = "Validation failed", errors });
            }

            try
            {
                var result = await _appointmentService.UpdateAppointment(appointmentId, appointment);
                _logger.Info(DateTime.Today.ToLongDateString()+" : Appointment updated successfully " +  appointmentId);

                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                _logger.Error(DateTime.Today.ToLongDateString()+" : Failed to update appointment with Id  " + appointmentId + " " + ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }
       



        [HttpGet("get/{appointmentId}")]
        [Authorize(Policy = Policy.RequireTruckingCompanyOrTerminalRole)]
        public async Task<IActionResult> GetAppointmentById(int appointmentId)
        {
            _logger.Info(DateTime.Today.ToLongDateString()+" : GetAppointment process strated for Id  " +  appointmentId);

            try
            {
                var result = await _appointmentService.GetAppointment(appointmentId);
                _logger.Info(DateTime.Today.ToLongDateString()+" : Fetched appointment successfully using  id " + appointmentId);

                return Ok(result);
            }
            catch (Exception ex) {
                _logger.Error(DateTime.Today.ToLongDateString()+" : Failed to fetch appointment details using appointmentId  " + ex.Message);
                return NotFound(new { message = ex.Message});
            }
        }
        
[HttpGet("terminals")]
[Authorize(Policy =Policy.RequireTruckingCompanyOrTerminalRole)]  // Adjust the authorization policy as needed
public async Task<IActionResult> GetAllTerminals()
{
    _logger.Info(DateTime.Today.ToLongDateString() + " : GetAllTerminals process started");

    try
    {
        var result = await _appointmentService.GetAllTerminals();
        _logger.Info(DateTime.Today.ToLongDateString() + " : Fetched all terminals successfully");

        return Ok(result);
    }
    catch (Exception ex)
    {
        _logger.Error(DateTime.Today.ToLongDateString() + " : Failed to fetch terminals " + ex.Message);
        return BadRequest(new { message = ex.Message });
    }
}

        [HttpGet("all")]
        [Authorize(Policy = Policy.RequireTruckingCompanyOrTerminalRole)]
        public async Task<IActionResult> GetAllAppointments([FromQuery] string format = "json")
        {
            _logger.Info(DateTime.Today.ToLongDateString()+" : GetAppointments process started");
            var result = await _appointmentService.GetAppointments();

            if (format.ToLower() == "excel")
            {
                var stream = GenerateExcelFile(result);
                string excelFileName = "Appointments.xlsx";

                stream.Position = 0;
                Response.Headers.Append("Content-Disposition", $"attachment; filename={excelFileName}");
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }

            _logger.Info(DateTime.Today.ToLongDateString()+" : Fetched appointments successfully"); 
            return Ok(result); // Return as JSON
        }

        private MemoryStream GenerateExcelFile(IEnumerable<CreateAppointmentDTO> appointments)
        {
            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.Add("Appointments");

                // Set headers
                worksheet.Cells[1, 1].Value = "Port Name";
                worksheet.Cells[1, 2].Value = "Address";
                worksheet.Cells[1, 3].Value = "City";
                worksheet.Cells[1, 4].Value = "State";
                worksheet.Cells[1, 5].Value = "Country";
                worksheet.Cells[1, 6].Value = "Trucking Company Name";
                worksheet.Cells[1, 7].Value = "GST No";
                worksheet.Cells[1, 8].Value = "Transport License No";
                worksheet.Cells[1, 9].Value = "Move Type"; // Fixed missing column index
                worksheet.Cells[1, 10].Value = "Container Number";
                worksheet.Cells[1, 11].Value = "Size Type";
                worksheet.Cells[1, 12].Value = "Line";
                worksheet.Cells[1, 13].Value = "Chassis No";
                worksheet.Cells[1, 14].Value = "Driver Name";
                worksheet.Cells[1, 15].Value = "Plate No";
                worksheet.Cells[1, 16].Value = "Phone Number";
                worksheet.Cells[1, 17].Value = "Appointment Status";
                worksheet.Cells[1, 18].Value = "Gate Code";

                // Add rows for each appointment
                int row = 2;
                foreach (var appointment in appointments)
                {
                    worksheet.Cells[row, 1].Value = appointment.PortName;
                    worksheet.Cells[row, 2].Value = appointment.Address;
                    worksheet.Cells[row, 3].Value = appointment.City;
                    worksheet.Cells[row, 4].Value = appointment.State;
                    worksheet.Cells[row, 5].Value = appointment.Country;
                    worksheet.Cells[row, 6].Value = appointment.TrCompanyName;
                    worksheet.Cells[row, 7].Value = appointment.GstNo;
                    worksheet.Cells[row, 8].Value = appointment.TransportLicNo;
                    worksheet.Cells[row, 9].Value = appointment.MoveType; // Fixed missing column index
                    worksheet.Cells[row, 10].Value = appointment.ContainerNumber;
                    worksheet.Cells[row, 11].Value = appointment.SizeType;
                    worksheet.Cells[row, 12].Value = appointment.Line;
                    worksheet.Cells[row, 13].Value = appointment.ChassisNo;
                    worksheet.Cells[row, 14].Value = appointment.DriverName;
                    worksheet.Cells[row, 15].Value = appointment.PlateNo;
                    worksheet.Cells[row, 16].Value = appointment.PhoneNumber;
                    worksheet.Cells[row, 17].Value = appointment.AppointmentStatus;
                    worksheet.Cells[row, 18].Value = appointment.GateCode;

                    row++;
                }

                package.Save();
            }

            stream.Position = 0;
            return stream;
        }

        [HttpDelete("delete/{appointmentId}")]
        [Authorize(Policy = Policy.RequireTruckingCompanyRole)]
        public async Task<IActionResult> DeleteAppointment(int appointmentId)
        {
            _logger.Info(DateTime.Today.ToLongDateString()+" : DeleteAppointment process started for Id"  + appointmentId);

            try
            {
                var result = await _appointmentService.DeleteAppointment(appointmentId);
                _logger.Info(DateTime.Today.ToLongDateString()+" :  appointment  deleted successfully  with Id " +  appointmentId);

                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                _logger.Error(DateTime.Today.ToLongDateString()+" : Failed to delete appointment with Id  " + appointmentId + " " + ex.Message);
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut("cancel/{appointmentId}")]
        [Authorize(Policy = Policy.RequireTerminalRole)]
        public async Task<IActionResult> CancelAppointment(int appointmentId)
        {
            _logger.Info(DateTime.Today.ToLongDateString()+" : CancelAppointment process started for Id " +  appointmentId);

            try
            {
                var result = await _appointmentService.CancelAppointment(appointmentId);
                _logger.Info(DateTime.Today.ToLongDateString()+" : Appointment canceled successfully with Id   " + appointmentId);

                return Ok(new { message = result });
            }
            catch (Exception ex) {
                _logger.Error(DateTime.Today.ToLongDateString()+" :  Failed to cancel appointment with Id " + appointmentId + " " + ex.Message);
                return NotFound(new { message = ex.Message});
            }
        }

        [HttpPut("approve/{appointmentId}")]
        [Authorize(Policy = Policy.RequireTerminalRole)]
        public async Task<IActionResult> ApproveAppointment(int appointmentId)
        {
            _logger.Info(DateTime.Today.ToLongDateString()+" : ApproveAppointment process started for Id"  +  appointmentId);

            try
            {
                var result = await _appointmentService.ApproveAppointment(appointmentId);
                _logger.Info(DateTime.Today.ToLongDateString()+" : Appointment approved successfully with Id " + appointmentId  );
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                _logger.Error(DateTime.Today.ToLongDateString()+" :  Failed to Approve appointment with Id " + appointmentId + " " + ex.Message);
                return NotFound(new { message = ex.Message });
            }
        }

    }
}
