using System.ComponentModel.DataAnnotations;

namespace advent_appointment_booking.DTOs
{
    public class Login
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
