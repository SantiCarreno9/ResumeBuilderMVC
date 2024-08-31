using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ResumeBuilder.Models.ViewModels
{
    public class VMPersonalInfo
    {
        [Required]
        [MaxLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [MaxLength(320)]
        [EmailAddress]
        public string? Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Column(TypeName = "varchar(20)")]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string? Address { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        [Display(Name = "Postal Code")]
        public string? PostalCode { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "LinkedIn URL")]
        public string? LinkedInURL { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Website URL")]
        public string? WebsiteURL { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "GitHub Account")]
        public string? GitHubAccount { get; set; }
    }
}
