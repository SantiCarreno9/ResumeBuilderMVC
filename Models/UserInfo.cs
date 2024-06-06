using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ResumeBuilder.Models
{
    public class UserInfo
    {
        [Key]
        public int UserId { get; set; }

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

        [Column(TypeName = "varchar(100)")]
        public string? LinkedInProfile { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string? WebSiteURL { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string? GitHubAccount { get; set; }

        public User? Account { get; set; }
    }
}
