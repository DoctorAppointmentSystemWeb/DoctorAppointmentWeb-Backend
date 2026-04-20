using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DoctorAppointmentSystem.Enum;

namespace DoctorAppointmentSystem.Models
{
    public class Doctor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Qualification { get; set; }

        public int ExperienceYears { get; set; }

        public AppointmentMode Mode { get; set; }

        [ForeignKey("Specialty")]
        public int SpecialtyId { get; set; }

        public Specialty Specialty { get; set; }
    }
}