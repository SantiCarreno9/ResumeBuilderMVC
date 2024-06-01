namespace ResumeBuilder.Models
{
    public class EducationRecord:Record
    {        
        public ICollection<ProfileEntry>? EducationRecords { get; set; }
    }
}
