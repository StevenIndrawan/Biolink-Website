using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BioLinkWeb.Models
{
    public class ProfileSettingViewModel
    {
        public string Username { get; set; }

        [Required]
        public string DisplayName { get; set; }

        public string Bio { get; set; }
        public string ProfileImageUrl { get; set; }
        public string Background { get; set; }

        public List<UserLink> Links { get; set; } = new List<UserLink>();
        public bool IsPublic { get; set; } = true;  // ✅ tambah
        public string Visibility { get; set; } = "public";

    }
}
