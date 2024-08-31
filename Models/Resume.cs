using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResumeBuilder.Models
{
    public class Resume
    {
        [Key]
        public string Id { get; set; }
        public string UserId { get; set; }
        [Column(TypeName = "nvarchar(1000)")]
        public string? ResumeInfo { get; set; }
        [Column(TypeName = "nvarchar(2000)")]
        public string? PersonalInfo { get; set; }
        [Column(TypeName = "nvarchar(4000)")]
        public string? ProfileEntries { get; set; }
    }
}
