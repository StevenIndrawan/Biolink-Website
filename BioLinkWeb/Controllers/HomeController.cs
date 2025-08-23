using BioLinkWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BioLinkWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Edit()
        {
            // Ambil user yang sedang login
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Login", "Account"); // atau ke halaman login

            return View(user); // ✅ return ApplicationUser
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ApplicationUser model, IFormFile? profileImage)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Login", "Account");

            // Update basic profile
            user.DisplayName = model.DisplayName;
            user.Bio = model.Bio;
            user.Background = model.Background;
            user.IsPublic = model.IsPublic;

            // Upload foto profil kalau ada
            if (profileImage != null && profileImage.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}_{profileImage.FileName}";
                var filePath = Path.Combine("wwwroot/images/profiles", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await profileImage.CopyToAsync(stream);
                }

                user.ProfileImageUrl = $"/images/profiles/{fileName}";
            }

            await _userManager.UpdateAsync(user);

            return RedirectToAction("Edit");
        }
    }
}
