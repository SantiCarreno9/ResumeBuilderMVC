using System.ComponentModel.DataAnnotations.Schema;

namespace ResumeBuilder.Models
{
    public class BasicResumeInfo
    {                
        [Column(TypeName = "nvarchar(50)")]
        public string? JobTitle { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string? Description { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public string? Skills { get; set; }
    }
}
