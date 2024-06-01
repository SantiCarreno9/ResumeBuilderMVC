using System.ComponentModel.DataAnnotations;

namespace ResumeBuilder.Models.ViewModels
{
    public class VMLogin
    {
        [EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }        
        public bool KeepLoggedIn { get; set; }
    }
}
