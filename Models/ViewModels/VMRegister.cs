using System.ComponentModel.DataAnnotations;

namespace ResumeBuilder.Models.ViewModels
{
    public class VMRegister
    {
        [EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
