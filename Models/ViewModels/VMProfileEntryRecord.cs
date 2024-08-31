namespace ResumeBuilder.Models.ViewModels
{
    public class VMProfileEntryRecord
    {
        public IEnumerable<ProfileEntry> ProfileEntries { get; set; }
        public bool Editable;
    }
}
