using System.ComponentModel.DataAnnotations;

namespace ResumeBuilder.Models
{
    public class ProfessionalInfo
    {
        [Key]
        public int UserId { get; set; }
        public ICollection<ProfileEntry>? ProfessionalRecord { get; set; }        
        public User? User { get; set; }
    }
}
