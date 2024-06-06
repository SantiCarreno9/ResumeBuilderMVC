using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResumeBuilder.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(320)]
        [EmailAddress]        
        public string Email { get; set; }
        [Required]
        [MaxLength(15)]
        [DataType(DataType.Password)]        
        public string Password { get; set; }

        [Required]
        [MaxLength(15)]
        [DataType(DataType.Password)]
        [NotMapped]
        public string ConfirmPassword { get; set; }

        [NotMapped]
        public bool KeepLoggedIn { get; set; }

        public UserInfo? UserInfo { get; set; }
        public ProfessionalInfo? ProfessionalInfo { get; set; }
    }
}
