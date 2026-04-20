using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace DoctorAppointmentSystem.Models
{
    public class Specialty
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Doctor> Doctors { get; set; }
    }
}