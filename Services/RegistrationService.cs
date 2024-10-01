using advent_appointment_booking.Database;
using advent_appointment_booking.Models;
using Microsoft.EntityFrameworkCore;

namespace advent_appointment_booking.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly ApplicationDbContext _databaseContext;

        public RegistrationService(ApplicationDbContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<bool> RegisterTruckingCompnay(TruckingCompany company)
        {
            // Validate if email is already taken
            if (await IsEmailExists(company.Email))
            {
                throw new Exception("Email already exists.");
            }

            // Validate if GST number is already taken
            if (await IsGstNoExists(company.GstNo))
            {
                throw new Exception("GST number already exists.");
            }

            // Validate if Transport License Number is already taken
            if (await IsTransportLicNoExists(company.TransportLicNo))
            {
                throw new Exception("Transport License Number already exists.");
            }

            // Add company to the database
            _databaseContext.TruckingCompanies.Add(company);
            await _databaseContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RegisterTerminal(Terminal terminal)
        {
            // Validate if email is already taken
            if (await IsEmailExists(terminal.Email))
            {
                throw new Exception("Email already exists.");
            }

            // Add terminal to the database
            _databaseContext.Terminals.Add(terminal);
            await _databaseContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> IsEmailExists(string email)
        {
            var existsInCompany = await _databaseContext.TruckingCompanies.AnyAsync(tc => tc.Email == email);
            var existsInTerminal = await _databaseContext.Terminals.AnyAsync(t => t.Email == email);

            return existsInCompany || existsInTerminal;
        }

        // New method to check if GST number already exists
        public async Task<bool> IsGstNoExists(string gstNo)
        {
            return await _databaseContext.TruckingCompanies.AnyAsync(tc => tc.GstNo == gstNo);
        }

        // New method to check if Transport License Number already exists
        public async Task<bool> IsTransportLicNoExists(string transportLicNo)
        {
            return await _databaseContext.TruckingCompanies.AnyAsync(tc => tc.TransportLicNo == transportLicNo);
        }
    }
}
