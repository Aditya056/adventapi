using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace advent_appointment_booking.Models
{
    public class Appointment
    {
        [Key]  
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AppointmentId { get; set; }

        [ForeignKey("TruckingCompany")]  
        public int TrCompanyId { get; set; }

        [ForeignKey("Terminal")]  
        public int TerminalId { get; set; }

        [ForeignKey("Driver")]
        public int DriverId { get; set; }

        [Required]
        public string MoveType { get; set; }
        [Required]
        public string ContainerNumber { get; set; }
        [Required]
        public string SizeType { get; set; }
        [Required]
        public string Line { get; set; }
        public string ChassisNo { get; set; }
        public string? AppointmentStatus { get; set; }
        
        public DateTime AppointmentCreated { get; set; }
        public DateTime AppointmentValidThrough { get; set; }
        public DateTime AppointmentLastModified { get; set; }
        public string? GateCode { get; set; }    

        public TruckingCompany? TruckingCompany { get; set; }
        public Terminal? Terminal { get; set; }
        public Driver? Driver { get; set; }
    }
}
