using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace advent_appointment_booking.Models
{
    public class Driver
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DriverId { get; set; }

        [ForeignKey("TruckingCompany")]  
        public int TrCompanyId { get; set; }

        public string DriverName { get; set; }
        public string PlateNo { get; set; }
        public string PhoneNumber { get; set; }
        [JsonIgnore]
        public TruckingCompany? TruckingCompany { get; set; }
        [JsonIgnore]
        public ICollection<Appointment>? Appointments { get; set; }
    }
}
