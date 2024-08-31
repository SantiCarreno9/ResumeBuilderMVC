using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ResumeBuilder.Models.ViewModels
{
    public class VMProfileEntry
    {
        [Key]
        public string Id { get; set; }        
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
    }
}
