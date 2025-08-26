using BioLinkWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BioLinkWeb.Controllers
{
    [Authorize] // hanya user login yang bisa akses
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            // Ambil user yang sedang login
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Redirect("/Identity/Account/Login"); // redirect ke login Identity

            return View(user); // ✅ return ApplicationUser ke View
        }
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApplicationUser model, IFormFile? profileImage)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Redirect("/Identity/Account/Login");

            // Update basic profile
            user.DisplayName = model.DisplayName;
            user.Bio = model.Bio;
            user.Background = model.Background;
            user.IsPublic = model.IsPublic;

            // Upload foto profil kalau ada
            if (profileImage != null && profileImage.Length > 0)
            {
                // pastikan folder ada
                var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/profiles");
                if (!Directory.Exists(uploadDir))
                    Directory.CreateDirectory(uploadDir);

                var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(profileImage.FileName)}";
                var filePath = Path.Combine(uploadDir, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await profileImage.CopyToAsync(stream);
                }

                user.ProfileImageUrl = $"/images/profiles/{fileName}";
            }

            // update user di database
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Gagal memperbarui profil");
                return View(user);
            }

            // setelah update redirect ke halaman biolink miliknya
            return Redirect("/" + user.UserName);
        }
    }
}
