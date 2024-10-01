namespace advent_appointment_booking.DTOs
{
    public class DriverDTO
    {
        public int DriverId { get; set; }
        public int TrCompanyId { get; set; }
        public string DriverName { get; set; }
        public string PlateNo { get; set; }
        public string PhoneNumber { get; set; }
        public TruckingCompanyDTO TruckingCompany { get; set; }
    }

    
}