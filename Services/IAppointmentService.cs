using advent_appointment_booking.DTOs;
using advent_appointment_booking.Models;

namespace advent_appointment_booking.Services
{
    public interface IAppointmentService
    {
        Task<CreateAppointmentDTO> CreateAppointment(Appointment appointment);
        Task<string> UpdateAppointment(int appointmentId, Appointment updatedAppointment);
    //    Task<string> UpdateAppointmentCreatedTime(int appointmentId, string newCreatedTime);

        Task<object> GetAppointment(int appointmentId);
    Task<List<CreateAppointmentDTO>> GetAppointments(DateTime? filterDate);

        Task<IEnumerable<CreateAppointmentDTO>> GetAppointments();
        Task<string> DeleteAppointment(int appointmentId);
        Task<string> CancelAppointment(int appointmentId);
        Task<string> ApproveAppointment(int appointmentId);
    Task<IEnumerable<TerminalDTO>> GetAllTerminals();
    }
}
