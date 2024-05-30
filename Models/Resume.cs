namespace ResumeBuilder.Models
{
    public class Resume
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Category {  get; set; }
        public string? Objective { get; set; }
        public List<string>? Skills { get; set; }
        public List<ProfileEntry>? EducationHistory { get; set; }
        public List<ProfileEntry>? WorkExperienceHistory { get; set; }
        public List<ProfileEntry>? ProjectsHistory { get; set; }
    }
}
