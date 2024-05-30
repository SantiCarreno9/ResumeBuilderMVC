using System.ComponentModel.DataAnnotations;

namespace ResumeBuilder.Models
{
    public class ProfileEntry
    {
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Organization { get; set; }
        [Required]
        public string? Location { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
        public bool IsCurrent { get; set; } = false;

        public List<string>? Details { get; set; }        
    }
}
