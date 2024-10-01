using advent_appointment_booking.Database;
using advent_appointment_booking.DTOs;
using advent_appointment_booking.Models;
using Microsoft.EntityFrameworkCore;

namespace advent_appointment_booking.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _databaseContext;

        public AppointmentService(ApplicationDbContext context)
        {
            _databaseContext = context;
        }

        // Create Appointment (Trucking Company only)
       public async Task<CreateAppointmentDTO> CreateAppointment(Appointment appointment)
{
    var truckingCompany = await _databaseContext.TruckingCompanies.FindAsync(appointment.TrCompanyId);
    if (truckingCompany == null)
        throw new Exception("Invalid Trucking Company.");

    var terminal = await _databaseContext.Terminals.FindAsync(appointment.TerminalId);
    if (terminal == null)
        throw new Exception("Invalid Terminal.");

    var driver = await _databaseContext.Drivers.FindAsync(appointment.DriverId);
    if (driver == null)
        throw new Exception("Driver does not exist.");

    var isContainerAlreadyScheduled = await _databaseContext.Appointments.AnyAsync(a => a.ContainerNumber == appointment.ContainerNumber);
    if(isContainerAlreadyScheduled)
    {
        throw new Exception("Appointment for the entered container number already exists.");
    }

    // Generate custom gate code: first two letters of Trucking Company + first three digits of Terminal ID
    string gateCode = $"{truckingCompany.TrCompanyName.Substring(0, 2).ToUpper()}{terminal.TerminalId.ToString().PadLeft(3, '0')}";
    appointment.GateCode = gateCode;

    // Use selected appointment date from the request
    appointment.AppointmentCreated = appointment.AppointmentCreated;  // Use the provided time slot
    appointment.AppointmentLastModified = DateTime.UtcNow;
    appointment.AppointmentValidThrough = appointment.AppointmentCreated.AddDays(2); // Example: 2 days valid
    appointment.AppointmentStatus = "Scheduled";

    await _databaseContext.Appointments.AddAsync(appointment);
    await _databaseContext.SaveChangesAsync();

    return new CreateAppointmentDTO
    {
        TrCompanyName = truckingCompany.TrCompanyName,
        GstNo = truckingCompany.GstNo,
        TransportLicNo = truckingCompany.TransportLicNo,
        PortName = terminal.PortName,
        Address = terminal.Address,
        City = terminal.City,
        State = terminal.State,
        Country = terminal.Country,
        DriverName = driver.DriverName,
        PlateNo = driver.PlateNo,
        PhoneNumber = driver.PhoneNumber,
        MoveType = appointment.MoveType,
        ContainerNumber = appointment.ContainerNumber,
        SizeType = appointment.SizeType,
        Line = appointment.Line,
        ChassisNo = appointment.ChassisNo,
        AppointmentStatus = appointment.AppointmentStatus,
        AppointmentCreated = appointment.AppointmentCreated,
        AppointmentValidThrough = appointment.AppointmentValidThrough,
        AppointmentLastModified = appointment.AppointmentLastModified,
        GateCode = appointment.GateCode
    };
}

 public async Task<IEnumerable<TerminalDTO>> GetAllTerminals()
{
    return await _databaseContext.Terminals
        .Select(t => new TerminalDTO
        {
            TerminalId = t.TerminalId,
            PortName = t.PortName,
            Address = t.Address,
            City = t.City,
            State = t.State,
            Country = t.Country,
            Email = t.Email,
            CreatedAt = t.CreatedAt,
            UpdatedAt = t.UpdatedAt
        })
        .ToListAsync();
}



        // Update Appointment (Trucking Company only)
        public async Task<string> UpdateAppointment(int appointmentId, Appointment updatedAppointment)
        {
            var appointment = await _databaseContext.Appointments.FindAsync(appointmentId);
            if (appointment == null )
                throw new Exception("Unauthorized or invalid appointment.");

            appointment.MoveType = updatedAppointment.MoveType;
            // appointment.ContainerNumber = updatedAppointment.ContainerNumber;
            // appointment.SizeType = updatedAppointment.SizeType;
            // appointment.Line = updatedAppointment.Line;
            // appointment.ChassisNo = updatedAppointment.ChassisNo;
            // appointment.TerminalId = updatedAppointment.TerminalId;
            // appointment.DriverId = updatedAppointment.DriverId;
            appointment.AppointmentLastModified = DateTime.UtcNow;
            appointment.AppointmentCreated=updatedAppointment.AppointmentCreated;
            _databaseContext.Appointments.Update(appointment);
            await _databaseContext.SaveChangesAsync();

            return "Appointment updated successfully.";
        }
      
// public async Task<string> UpdateAppointmentCreatedTime(int appointmentId, DateTime newCreatedTime)
// {
//     // Find the appointment by its ID
//     var appointment = await _databaseContext.Appointments.FindAsync(appointmentId);
    
//     if (appointment == null)
//     {
//         throw new Exception("Appointment not found.");
//     }

//     // Update only the AppointmentCreated field
//     appointment.AppointmentCreated = newCreatedTime;
//     appointment.AppointmentLastModified = DateTime.UtcNow;

//     // Mark the appointment as updated and save the changes
//     _databaseContext.Appointments.Update(appointment);
//     await _databaseContext.SaveChangesAsync();

//     return "Appointment created time updated successfully.";
// }



        // Get Appointment (Accessible to both Trucking Company and Terminal)
        public async Task<object> GetAppointment(int appointmentId)
        {
            var appointment = await _databaseContext.Appointments
                .Select(a => new
                {
                    a.AppointmentId,
                    a.ContainerNumber,
                    a.SizeType,
                    a.Line,
                    a.ChassisNo,
                    a.GateCode,
                    a.AppointmentCreated,
                    a.AppointmentStatus,
                    a.AppointmentValidThrough,
                    a.TruckingCompany.TrCompanyName,
                    a.TruckingCompany.Email,
                    a.TruckingCompany.GstNo,
                    a.TruckingCompany.TransportLicNo,
                    a.Terminal.PortName,
                    a.Terminal.City,
                    a.Terminal.State,
                    a.Terminal.Country,
                    a.Terminal.Address,
                    a.Driver.DriverName,
                    a.Driver.PlateNo,
                    a.Driver.PhoneNumber
                })
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);

            if (appointment == null)
                throw new Exception("Appointment not found.");

            return appointment;
        }

        // Get All Appointments (Accessible to both Trucking Company and Terminal)
        public async Task<IEnumerable<CreateAppointmentDTO>> GetAppointments()
        {
            return await _databaseContext.Appointments
                .Select(a => new CreateAppointmentDTO
                {
                    PortName = a.Terminal.PortName,
                    Address = a.Terminal.Address,
                    City = a.Terminal.City,
                    State = a.Terminal.State,
                    Country = a.Terminal.Country,
                    TrCompanyName = a.TruckingCompany.TrCompanyName,
                    GstNo = a.TruckingCompany.GstNo,
                    TransportLicNo = a.TruckingCompany.TransportLicNo,
                    Email=a.TruckingCompany.Email,
                    MoveType = a.MoveType,
                    ContainerNumber = a.ContainerNumber,
                    SizeType = a.SizeType,
                    Line = a.Line,
                    Appointmentid=a.AppointmentId,
                    ChassisNo = a.ChassisNo,
                    DriverName = a.Driver.DriverName,
                    PlateNo = a.Driver.PlateNo,
                    PhoneNumber = a.Driver.PhoneNumber,
                    AppointmentStatus = a.AppointmentStatus,
                    AppointmentCreated = a.AppointmentCreated,
                    AppointmentValidThrough = a.AppointmentValidThrough,
                    AppointmentLastModified = a.AppointmentLastModified,
                    GateCode = a.GateCode
                })
                .ToListAsync();
        }

        // Delete Appointment (Trucking Company only)
        public async Task<string> DeleteAppointment(int appointmentId)
        {
            var appointment = await _databaseContext.Appointments.FindAsync(appointmentId);
            if (appointment == null)
                throw new Exception("Appointment not found.");

            _databaseContext.Appointments.Remove(appointment);
            await _databaseContext.SaveChangesAsync();

            return "Appointment deleted successfully.";
        }

        // Cancel Appointment (Terminal only)
        public async Task<string> CancelAppointment(int appointmentId)
        {
            var appointment = await _databaseContext.Appointments.FindAsync(appointmentId);
            if (appointment == null)
                throw new Exception("Appointment not found.");

            appointment.AppointmentStatus = "Canceled";

            _databaseContext.Appointments.Update(appointment);
            await _databaseContext.SaveChangesAsync();

            return "Appointment canceled successfully.";
        }

        public async Task<string> ApproveAppointment(int appointmentId)
        {
            var appointment = await _databaseContext.Appointments.FindAsync(appointmentId);
            if (appointment == null)
                throw new Exception("Appointment not found.");

            // Update the status to 'Approved'
            appointment.AppointmentStatus = "Approved";

            _databaseContext.Appointments.Update(appointment);
            await _databaseContext.SaveChangesAsync();

            return "Appointment approved successfully.";
        }

    }
}
