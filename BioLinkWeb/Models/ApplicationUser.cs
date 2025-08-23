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
        public List<Link> Links { get; set; } = new();
    }
}
