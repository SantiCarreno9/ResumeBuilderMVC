using System.ComponentModel.DataAnnotations;

namespace ResumeBuilder.Models
{
    public class ProfileInfo
    {
        [Key]
        public int AccountId { get; set; }
        public Account? Account { get; set; }
        public PersonalInfo? PersonalInfo {  get; set; }
        public ICollection<ProfileEntry>? ProfessionalRecord { get; set; }
    }
}
