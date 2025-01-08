using System.ComponentModel.DataAnnotations;

namespace ResumeBuilder.Models
{
    public class AdditionalContact
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Value { get; set; }
        public string? DisplayName { get; set; }
        public string? Icon { get; set; }
        public bool IsHyperlink { get; set; } = true;

        public string GetDisplayName()
        {
            if (DisplayName != null)
                return DisplayName;

            if (Name != null)
                return Name;

            return string.Empty;
        }

        public string GetIcon()
        {
            if (Icon != null)
                return Icon;

            if (Name == null)
                return "<i class=\"fa-solid fa-address-book\"></i>";

            return Name switch
            {
                "LinkedIn" => "<i class=\"fab fa-linkedin\" aria-hidden=\"true\"></i>",
                "GitHub" => "<i class=\"fab fa-github\" aria-hidden=\"true\"></i>",
                "Website" => "<i class=\"fa-solid fa-globe\" aria-hidden=\"true\"></i>",
                "Portfolio" => "<i class=\"fa-solid fa-briefcase\" aria-hidden=\"true\"></i>",
                _ => "<i class=\"fa-solid fa-address-book\"></i>"
            };
        }

        public AdditionalContact(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
