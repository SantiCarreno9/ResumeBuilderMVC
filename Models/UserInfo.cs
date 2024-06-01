using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ResumeBuilder.Models
{
    public class UserInfo : Record
    {
        [Required]
        [MaxLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        
        [Required]
        [EmailAddress]
        [MaxLength(320)]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Column(TypeName = "varchar(20)")]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; } = string.Empty;

        [Column(TypeName = "varchar(100)")]
        public string? Address { get; set; } = string.Empty;        
    }
}
