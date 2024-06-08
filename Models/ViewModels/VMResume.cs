namespace ResumeBuilder.Models.ViewModels
{
    public class VMResume
    {
        public string? ResumeName { get; set; }        
        public string? JobTitle { get; set; }
        public string? Skills { get; set; }
        public string? Description { get; set; }
        public PersonalInfo? PersonalInfo { get; set; }
        public ICollection<ProfileEntry>? ProfessionalRecord { get; set; }
    }
}
