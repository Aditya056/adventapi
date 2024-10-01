using advent_appointment_booking.Database;
using advent_appointment_booking.DTOs;
using advent_appointment_booking.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace advent_appointment_booking.Services
{
    public class DriverService : IDriverService
    {
        private readonly ApplicationDbContext _context;

        public DriverService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Create a new driver with a check for existing plate number and phone number
        public async Task<Driver> CreateDriver(Driver driver)
        {
            // Check if the TruckingCompany exists
            var truckingCompany = await _context.TruckingCompanies
                .FirstOrDefaultAsync(tc => tc.TrCompanyId == driver.TrCompanyId);

            if (truckingCompany == null)
            {
                throw new Exception("Trucking company doesn't exist");
            }

            // Check if a driver with the same plate number or phone number already exists
            var existingDriver = await _context.Drivers
                .FirstOrDefaultAsync(d => d.PlateNo == driver.PlateNo || d.PhoneNumber == driver.PhoneNumber);

            if (existingDriver != null)
            {
                // If a driver with the same plate number exists
                if (existingDriver.PlateNo == driver.PlateNo)
                {
                    throw new Exception("Driver with the same plate number already exists");
                }

                // If a driver with the same phone number exists
                if (existingDriver.PhoneNumber == driver.PhoneNumber)
                {
                    throw new Exception("Driver with the same phone number already exists");
                }
            }

            // Proceed to add the driver if all validations pass
            _context.Drivers.Add(driver);
            await _context.SaveChangesAsync();
            return driver;
        }


        // Other CRUD operations for Driver (Get, Update, Delete)
        public async Task<DriverDTO> GetDriver(int driverId)
        {
            var driver = await _context.Drivers
                .Include(d => d.TruckingCompany)
                .FirstOrDefaultAsync(d => d.DriverId == driverId);

            if (driver == null)
            {
                throw new Exception("Driver not found");
            }

            // Map the Driver entity to DriverDTO
            var driverDto = new DriverDTO
            {
                DriverId = driver.DriverId,
                TrCompanyId = driver.TrCompanyId,
                DriverName = driver.DriverName,
                PlateNo = driver.PlateNo,
                PhoneNumber = driver.PhoneNumber,
                TruckingCompany = new TruckingCompanyDTO
                {
                    TrCompanyId = driver.TruckingCompany.TrCompanyId,
                    TrCompanyName = driver.TruckingCompany.TrCompanyName,
                    GstNo = driver.TruckingCompany.GstNo,
                    TransportLicNo = driver.TruckingCompany.TransportLicNo,
                    Email = driver.TruckingCompany.Email,
                    CreatedAt = driver.TruckingCompany.CreatedAt,
                    UpdatedAt = driver.TruckingCompany.UpdatedAt
                }
            };

            return driverDto;
        }

        public async Task<IEnumerable<DriverDTO>> GetAllDrivers()
        {
            var drivers = await _context.Drivers
                .Include(d => d.TruckingCompany)
                .ToListAsync();

            // Map each Driver entity to DriverDTO to remove circular references
            var driverDtos = drivers.Select(driver => new DriverDTO
            {
                DriverId = driver.DriverId,
                TrCompanyId = driver.TrCompanyId,
                DriverName = driver.DriverName,
                PlateNo = driver.PlateNo,
                PhoneNumber = driver.PhoneNumber,
                TruckingCompany = new TruckingCompanyDTO
                {
                    TrCompanyId = driver.TruckingCompany.TrCompanyId,
                    TrCompanyName = driver.TruckingCompany.TrCompanyName,
                    GstNo = driver.TruckingCompany.GstNo,
                    TransportLicNo = driver.TruckingCompany.TransportLicNo,
                    Email = driver.TruckingCompany.Email,
                    CreatedAt = driver.TruckingCompany.CreatedAt,
                    UpdatedAt = driver.TruckingCompany.UpdatedAt
                }
            }).ToList();  

            return driverDtos;
        }

        public async Task<string> UpdateDriver(int driverId, Driver driver)
        {
            var existingDriver = await _context.Drivers.FindAsync(driverId);

            if (existingDriver == null)
            {
                return "Driver not found";
            }

            existingDriver.DriverName = driver.DriverName;
            existingDriver.PlateNo = driver.PlateNo;
            existingDriver.PhoneNumber = driver.PhoneNumber;

            _context.Drivers.Update(existingDriver);
            await _context.SaveChangesAsync();

            return "Driver updated successfully";
        }

        public async Task<string> DeleteDriver(int driverId)
        {
            var driver = await _context.Drivers.FindAsync(driverId);

            if (driver == null)
            {
                return "Driver not found";
            }

            _context.Drivers.Remove(driver);
            await _context.SaveChangesAsync();

            return "Driver deleted successfully";
        }
    }
}
