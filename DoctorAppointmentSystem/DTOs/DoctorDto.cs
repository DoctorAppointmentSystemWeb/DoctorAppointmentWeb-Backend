using DoctorAppointmentSystem.Enum;

namespace DoctorAppointmentSystem.DTOs
{
    public class DoctorDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Qualification { get; set; }
        public int ExperienceYears { get; set; }
        public AppointmentMode Mode { get; set; }
        public int SpecialtyId { get; set; }
    }
}