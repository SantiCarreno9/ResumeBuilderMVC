using System.ComponentModel.DataAnnotations;

namespace ResumeBuilder.Models
{
    public class AdditionalContact
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Value { get; set; }
    }
}
