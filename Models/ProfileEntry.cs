using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResumeBuilder.Models
{
    public enum EntryCategory
    {
        Education,
        WorkExperience,
        Project,
        Other
    }
    public class ProfileEntry
    {
        public int Id { get; set; }
        public int ProfessionalInfoId { get; set; }
        public EntryCategory Category { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string? Title { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string? Organization { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string? Location { get; set; }

        //[DataType(DataType.Date),DisplayFormat(ApplyFormatInEditMode =true,DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }
        //[DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
        public bool IsCurrent { get; set; } = false;
               
        [Column(TypeName = "nvarchar(4000)")]
        public string? Details { get; set; }
    }
}
