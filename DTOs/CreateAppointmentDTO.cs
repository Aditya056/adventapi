namespace advent_appointment_booking.DTOs
{
    public class CreateAppointmentDTO
    {
        public string PortName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        public string Email { get; set; }
        public string PlateNo { get; set; }
        public string PhoneNumber { get; set; }
        public string TrCompanyName { get; set; }
        public string GstNo { get; set; }
        public string TransportLicNo { get; set; }
        public string DriverName { get; set; }
        public string MoveType { get; set; }
        public int Appointmentid { get; set; }
        public string ContainerNumber { get; set; }
        public string SizeType { get; set; }
        public string Line { get; set; }
        public string ChassisNo { get; set; }
        public string AppointmentStatus { get; set; }
        public DateTime AppointmentCreated { get; set; }
        public DateTime AppointmentValidThrough { get; set; }
        public DateTime AppointmentLastModified { get; set; }
        public string? GateCode { get; set; }
    }
}
