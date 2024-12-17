namespace ResumeBuilder.Models.ViewModels
{
    public class VMResume
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ResumeBasicInfo? ResumeInfo { get; set; }
        public PersonalInfo? PersonalInfo { get; set; }
        public List<ProfileEntry>? ProfileEntries { get; set; }
    }
}
