using advent_appointment_booking.Database;
using advent_appointment_booking.Enums;
using advent_appointment_booking.Helpers;
using Azure.Core;
using Microsoft.EntityFrameworkCore;

namespace advent_appointment_booking.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _databaseContext;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public AuthService(ApplicationDbContext databaseContext, JwtTokenGenerator jwtTokenGenerator)
        {
            _databaseContext = databaseContext;
            _jwtTokenGenerator = jwtTokenGenerator;
        }
    
        public async Task<object> LoginAsync(string email, string password, string userType)
        {
            if (userType == UserType.TruckingCompany)
            {
                var trCompany = await _databaseContext.TruckingCompanies
                    .Where(tc => tc.Email == email && tc.Password == password)
                    .Select(trCompany => new
                    {
                        trCompany.TrCompanyId,
                        trCompany.TrCompanyName,
                        trCompany.Email,
                        trCompany.TransportLicNo,
                        trCompany.GstNo,
                        trCompany.CreatedAt,
                        trCompany.UpdatedAt
                    })
                    .FirstOrDefaultAsync();

                if (trCompany == null)
                {
                    throw new Exception("Invalid credentials.");
                }

                var token = _jwtTokenGenerator.GenerateToken(email, userType);
                return new {data = trCompany, token}; 
            }
            else if (userType == UserType.Terminal)
            {
                var terminal = await _databaseContext.Terminals
                    .Where(t => t.Email == email && t.Password == password)
                    .Select(tr => new
                    {
                        tr.PortName,
                        tr.Email,
                        tr.Address,
                        tr.City,
                        tr.State,
                        tr.Country,
                        tr.CreatedAt,
                        tr.UpdatedAt
                    })
                    .FirstOrDefaultAsync();

                if (terminal == null)
                {
                    throw new Exception("Invalid credentials.");
                }

                var token = _jwtTokenGenerator.GenerateToken(email, userType);
                return new { data = terminal, token }; 
            }

            throw new Exception("Invalid user type.");
        }
    }
}
