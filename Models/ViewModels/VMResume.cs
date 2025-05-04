namespace ResumeBuilder.Models.ViewModels
{
    public class VMResume
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public VMResumeBasicInfo? ResumeInfo { get; set; }
        public VMPersonalInfo? PersonalInfo { get; set; }
        public List<ProfileEntry>? ProfileEntries { get; set; }
        public EntryCategory[] OrderedCategories { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
