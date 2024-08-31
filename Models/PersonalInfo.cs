using ResumeBuilder.Models.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace ResumeBuilder.Models
{
    public class PersonalInfo : VMPersonalInfo
    {        
        [Key]
        public string UserId { get; set; }                
    }
}
