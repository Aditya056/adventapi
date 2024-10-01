using advent_appointment_booking.DTOs;
using advent_appointment_booking.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace advent_appointment_booking.Services
{
    public interface IDriverService
    {
        Task<Driver> CreateDriver(Driver driver);
        Task<DriverDTO> GetDriver(int driverId);
        Task<IEnumerable<DriverDTO>> GetAllDrivers();
        Task<string> UpdateDriver(int driverId, Driver driver);
        Task<string> DeleteDriver(int driverId);
    }
}
