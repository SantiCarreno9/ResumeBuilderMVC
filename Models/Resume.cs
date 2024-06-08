using System.ComponentModel.DataAnnotations.Schema;

namespace ResumeBuilder.Models
{
    public class Resume
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string? ResumeName { get; set; }
        [Column(TypeName = "nvarchar(4000)")]
        public string? PersonalInfo { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string? JobTitle { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string? Description { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public string? Skills { get; set; }
        [Column(TypeName = "nvarchar(4000)")]
        public string? EducationRecord { get; set; }
        [Column(TypeName = "nvarchar(4000)")]
        public string? ExperienceRecord { get; set; }
    }
}
