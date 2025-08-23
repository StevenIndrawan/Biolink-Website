using System.Collections.Generic;

namespace BioLinkWeb.Models
{
    public class BioLinkViewModel
    {
        public string UserName { get; set; }
        public string Bio { get; set; }
        public string ProfileImageUrl { get; set; }

        public string BackgroundImage { get; set; }
        public string BackgroundColor { get; set; } = "#111827";
        public string GradientStart { get; set; } = "#4f46e5";
        public string GradientEnd { get; set; } = "#9333ea";

        public List<LinkItem> Links { get; set; } = new();
    }

    public class LinkItem
    {
        public string Title { get; set; }
        public string Url { get; set; }
    }
}
