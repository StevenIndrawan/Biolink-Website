using Microsoft.AspNetCore.Mvc;
using BioLinkWeb.Models;

namespace BioLinkWeb.Controllers
{
    public class BiolinkController : Controller
    {
        [Route("{username}")]
        public IActionResult Index(string username)
        {
            var user = UserStore.CurrentUser;

            // Jika user belum ada, inisialisasi default
            if (user == null)
            {
                user = new User
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

                UserStore.CurrentUser = user;
            }

            // validasi username & visibility
            if (!string.Equals(user.Username, username, StringComparison.OrdinalIgnoreCase))
            {
                return NotFound();
            }

            if (!user.IsPublic)
            {
                return NotFound(); // 🔒 private → 404
            }

            return View(user);
        }

        [HttpGet]
        public IActionResult Settings()
        {
            var model = new ProfileSettingViewModel
            {
                Username = UserStore.CurrentUser.Username,
                DisplayName = UserStore.CurrentUser.DisplayName,
                Bio = UserStore.CurrentUser.Bio,
                ProfileImageUrl = UserStore.CurrentUser.ProfileImageUrl,
                Background = UserStore.CurrentUser.Background,
                IsPublic = UserStore.CurrentUser.IsPublic, // ✅ ambil status public/private
                Links = UserStore.CurrentUser.Links
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Settings(ProfileSettingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // update ke UserStore
            UserStore.CurrentUser.Username = model.Username;
            UserStore.CurrentUser.DisplayName = model.DisplayName;
            UserStore.CurrentUser.Bio = model.Bio;
            UserStore.CurrentUser.ProfileImageUrl = model.ProfileImageUrl;
            UserStore.CurrentUser.Background = model.Background;
            UserStore.CurrentUser.IsPublic = model.IsPublic; // ✅ simpan setting
            UserStore.CurrentUser.Links = model.Links ?? new List<Link>();

            TempData["Message"] = "Profile berhasil disimpan!";

            return RedirectToAction("Index", new { username = model.Username });
        }
    }
}
