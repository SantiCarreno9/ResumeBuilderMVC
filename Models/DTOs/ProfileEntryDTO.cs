using System.ComponentModel.DataAnnotations;

namespace ResumeBuilder.Models.DTOs
{
    public class ProfileEntryDTO
    {
        public int Id { get; set; }        
        public string? Title { get; set; }        
        public string? Organization { get; set; }        
        public string? Location { get; set; }        
        public DateTime StartDate { get; set; }       
        public DateTime? EndDate { get; set; }
        public bool IsCurrent { get; set; } = false;
        public List<string> Details { get; set; }
    }
}
