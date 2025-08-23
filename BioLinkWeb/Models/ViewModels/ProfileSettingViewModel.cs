namespace BioLinkWeb.Models.ViewModels
{
    public class ProfileSettingViewModel
    {
        public string Username { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public string Bio { get; set; } = "";
        public string ProfileImageUrl { get; set; } = "";
        public string Background { get; set; } = "";

        public List<Link> Links { get; set; } = new List<Link>();
    }
}
