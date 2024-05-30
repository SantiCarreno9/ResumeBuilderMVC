using System.ComponentModel.DataAnnotations;

namespace ResumeBuilder.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; } = string.Empty;        
        public string? Address { get; set; } = string.Empty;
        public List<ProfileEntry>? EducationRecords { get; set; }
        public List<ProfileEntry>? WorkExperienceRecords { get; set; }
        public List<ProfileEntry>? ProjectRecords { get; set; }
    }
}
