using BioLinkWeb.Models;

namespace BioLinkWeb.Data
{
    public static class UserStore
    {
        public static User CurrentUser { get; set; } = new User
        {
            Username = "steven",
            DisplayName = "Steven",
            Bio = "Selamat datang di bio saya 👋",
            ProfileImageUrl = "/images/profile.png",
            Background = "linear-gradient(to right, #6a11cb, #2575fc)",
            IsPublic = true,
            Links = new List<Link>
            {
                new Link { Id = 1, Title = "Website", Url = "https://steventi.wordpress.com/", Icon="🌐", Order=1 },
                new Link { Id = 2, Title = "Instagram", Url = "https://www.instagram.com/stevens.010/", Icon="📸", Order=2 }
            }
        };
    }
}
