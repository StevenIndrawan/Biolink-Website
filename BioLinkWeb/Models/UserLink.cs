using System.ComponentModel.DataAnnotations;

namespace BioLinkWeb.Models
{
    public class UserLink
    {
        public int Id { get; set; }

        [Required]
        public string? Title { get; set; }

        [Required]
        public string? Url { get; set; }

        public string? Icon { get; set; } = "🔗";

        public int Order { get; set; }

        // Relasi ke User
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
