using Microsoft.AspNetCore.Identity;

namespace BioLinkWeb.Models
{
    public class User
    {
        public string Username { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public string Bio { get; set; } = "";
        public string ProfileImageUrl { get; set; } = "";
        public string Background { get; set; } = "";
        public bool IsPublic { get; set; } = true;
        public bool IsActive { get; set; } = true;

        // Relasi ke links
        public List<Link> Links { get; set; } = new List<Link>();
    }
}
