using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BioLinkWeb.Models.ViewModels
{
    public class ProfileSettingViewModel
    {
        public string Username { get; set; }

        [Display(Name = "Nama Tampilan")]
        public string DisplayName { get; set; }

        [Display(Name = "Bio")]
        public string Bio { get; set; }

        [Display(Name = "Foto Profil")]
        public string ProfileImageUrl { get; set; }

        [Display(Name = "Background")]
        public string Background { get; set; }

        [Display(Name = "Publik?")]
        public bool IsPublic { get; set; }

        public List<UserLink> Links { get; set; } = new List<UserLink>();
    }
}
