namespace advent_appointment_booking.Services
{
    public interface IAuthService
    {
        Task<object> LoginAsync(string email, string password, string userType);
    }
}
