using ResumeBuilder.Models.ViewModels;

namespace ResumeBuilder.Models
{
    public enum EntryCategory
    {
        Education,
        WorkExperience,
        Project,
        Other
    }

    public class ProfileEntry : VMProfileEntry
    {        
        public string? UserId { get; set; }        
    }
}
