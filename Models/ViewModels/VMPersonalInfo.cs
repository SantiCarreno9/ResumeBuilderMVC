using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ResumeBuilder.Models.ViewModels
{
    public class VMPersonalInfo
    {        
        [MaxLength(50)]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }
     
        [MaxLength(50)]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [MaxLength(320)]
        [EmailAddress]
        public string? Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Column(TypeName = "varchar(20)")]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        public string? Address { get; set; }        

        [Column(TypeName = "nvarchar(4000)")]
        [Display(Name = "GitHub Account")]
        public string? AdditionalContactInfo { get; set; }
    }
}
