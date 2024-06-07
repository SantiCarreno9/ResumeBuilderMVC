using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ResumeBuilder.Models
{
    public class PersonalInfo
    {
        [Key]
        public int ProfileInfoId { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Column(TypeName = "varchar(20)")]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string? Address { get; set; }

        [Column(TypeName = "varchar(10)")]
        [Display(Name = "Postal Code")]
        public string? PostalCode { get; set; }

        [Column(TypeName = "varchar(100)")]
        [Display(Name = "LinkedIn URL")]
        public string? LinkedInURL { get; set; }

        [Column(TypeName = "varchar(100)")]
        [Display(Name = "Website URL")]
        public string? WebSiteURL { get; set; }

        [Column(TypeName = "varchar(100)")]
        [Display(Name = "GitHub Account")]
        public string? GitHubAccount { get; set; }

        public ProfileInfo? ProfileInfo { get; set; }
    }
}
