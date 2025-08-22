using System.ComponentModel.DataAnnotations;

namespace BioLinkWeb.Models
{
    public class Profile
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string DisplayName { get; set; } = string.Empty;

        [StringLength(250)]
        public string Bio { get; set; } = string.Empty;

        public string? ProfilePictureUrl { get; set; }
    }
}
