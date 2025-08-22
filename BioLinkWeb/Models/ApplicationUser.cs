using Microsoft.AspNetCore.Identity;

namespace BioLinkApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? DisplayName { get; set; }
        public string? Bio { get; set; }
        public string? AvatarUrl { get; set; }

        public ICollection<Link> Links { get; set; } = new List<Link>();
    }
}