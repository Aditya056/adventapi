using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace advent_appointment_booking.Models
{
    public class TruckingCompany
    {
        [Key]  
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TrCompanyId { get; set; }

        [Required]  
        [MaxLength(200)]  
        public string TrCompanyName { get; set; }

        [MaxLength(50)]
        public string GstNo { get; set; }

        [MaxLength(50)]
        public string TransportLicNo { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Appointment>? Appointments { get; set; }
        public ICollection<Driver>? Drivers { get; set; }

    }
}
