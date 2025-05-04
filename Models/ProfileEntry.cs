using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResumeBuilder.Models
{
    public enum EntryCategory
    {
        Education,
        WorkExperience,
        Project,
        Volunteer,
        Other
    }

    public class ProfileEntry
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public EntryCategory Category { get; set; } = EntryCategory.WorkExperience;
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string? Title { get; set; }
        
        [Column(TypeName = "nvarchar(100)")]
        public string? Organization { get; set; }
        
        [Column(TypeName = "nvarchar(100)")]
        public string? Location { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "From")]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "To")]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Is Current")]
        public bool IsCurrent { get; set; } = false;

        [Column(TypeName = "nvarchar(4000)")]
        public string? Details { get; set; }

        [Column(TypeName = "nvarchar(450)")]
        public string? UserId { get; set; }        

        public ProfileEntry()
        {
            Id = Guid.NewGuid().ToString();
        }
        public static string GetCategoryName(EntryCategory category)
        {                        
            return category switch
            {
                EntryCategory.WorkExperience => "Experience",
                _ => category.ToString()
            };
        }
    }
}
