namespace advent_appointment_booking.DTOs
{
    public class TerminalDTO
    {
        public int TerminalId { get; set; }
        public string PortName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
