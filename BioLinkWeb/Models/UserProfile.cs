using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BioLinkWeb.Models
{
    public class UserProfile
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string DisplayName { get; set; } = string.Empty;

        [MaxLength(250)]
        public string Bio { get; set; } = string.Empty;

        public string? ProfileImageUrl { get; set; }

        public IdentityUser? User { get; set; }
    }
}
