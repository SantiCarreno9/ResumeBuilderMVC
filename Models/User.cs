using System.ComponentModel.DataAnnotations;

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

        public UserInfo? UserInfo { get; set; }
    }
}
