namespace BioLinkWeb.Models
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
                new Link { Id = 2, Title = "Instagram", Url = "https://www.instagram.com/stevens.010/", Icon="📸", Order=2 },
                new Link { Id = 3, Title = "Twitter", Url = "https://twitter.com/", Icon="🐦", Order=3 },
                new Link { Id = 4, Title = "LinkedIn", Url = "https://www.linkedin.com/in/steven", Icon="💼", Order=4 },
                new Link { Id = 5, Title = "Github", Url = "https://github.com/steven", Icon="🐙", Order=5 },
                new Link { Id = 6, Title = "YouTube", Url = "https://youtube.com", Icon="▶️", Order=6 }
            }
        };
    }
}
