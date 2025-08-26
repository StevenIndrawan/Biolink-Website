using Microsoft.AspNetCore.Identity;

namespace BioLinkWeb.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? DisplayName { get; set; }
        public string? Bio { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? Background { get; set; }
        public bool IsPublic { get; set; } = true;
        public bool IsActive { get; set; } = true;
        public bool IsBioPublic { get; set; } = true;
        public string Visibility { get; set; } = "public";

        public UserProfile? Profile { get; set; }
        public virtual ICollection<UserLink> Links { get; set; } = new List<UserLink>();
    }

}
