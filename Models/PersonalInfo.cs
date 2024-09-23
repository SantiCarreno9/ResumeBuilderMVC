using ResumeBuilder.Models.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResumeBuilder.Models
{
    public class PersonalInfo : VMPersonalInfo
    {        
        [Key]
        public string UserId { get; set; }        
    }
}
