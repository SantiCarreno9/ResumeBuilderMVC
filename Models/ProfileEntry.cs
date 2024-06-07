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
        public int ProfileInfoId { get; set; }
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
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "From")]
        public DateTime? StartDate { get; set; }
        //[DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "To")]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Is Current")]
        public bool IsCurrent { get; set; } = false;

        [Column(TypeName = "nvarchar(4000)")]
        public string? Details { get; set; }
    }
}
