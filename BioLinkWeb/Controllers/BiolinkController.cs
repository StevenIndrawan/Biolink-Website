using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BioLinkWeb.Data;
using BioLinkWeb.Models;
using System.Linq;
using System.Threading.Tasks;

namespace BioLinkWeb.Controllers
{
    [Authorize] // default harus login, kecuali yang [AllowAnonymous]
    [Route("Biolink")] // semua endpoint di controller ini prefiks /Biolink
    public class BiolinkController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public BiolinkController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        
        // =====================================================
        // 🔹 Search User: /{Search}
        // =====================================================
        [AllowAnonymous]
        [HttpGet("Search")]
        public async Task<IActionResult> Search(string? q)
        {
            var users = _userManager.Users
                .Where(u => u.IsPublic) // hanya tampilkan user yang public
                .AsQueryable();

            if (!string.IsNullOrEmpty(q))
            {
                users = users.Where(u => u.UserName.Contains(q) || u.DisplayName.Contains(q));
            }

            var result = await users
                .OrderBy(u => u.DisplayName)
                .Select(u => new UserViewModel
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    DisplayName = u.DisplayName,
                    Bio = u.Bio,
                    ProfileImageUrl = string.IsNullOrEmpty(u.ProfileImageUrl) ? "/images/profile.png" : u.ProfileImageUrl
                })
                .ToListAsync();

            return View(result); // Views/Biolink/Search.cshtml
        }
        // =====================================================
        // 🔹 Public Profile: /{username}
        // =====================================================
        [AllowAnonymous]
        [HttpGet("/" + "{username}")] // root-level username
        public async Task<IActionResult> Index(string username)
        {
            if (string.IsNullOrEmpty(username))
                return NotFound();

            var user = await _userManager.Users
                .Include(u => u.Links)
                .FirstOrDefaultAsync(u => u.UserName == username);

            if (user == null)
                return NotFound();

            // kalau private, hanya pemilik yang bisa lihat
            if (!user.IsPublic)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser == null || currentUser.Id != user.Id)
                {
                    return NotFound();
                }
            }

            return View(user); // Views/Biolink/Index.cshtml
        }


        // =====================================================
        // 🔹 GET Settings: /Biolink/Settings
        // =====================================================
        [HttpGet("Settings")]
        public async Task<IActionResult> Settings()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account", new { area = "Identity" });

            await _context.Entry(user).Collection(u => u.Links).LoadAsync();

            var vm = new ProfileSettingViewModel
            {
                Username = user.UserName,
                DisplayName = user.DisplayName,
                Bio = user.Bio,
                ProfileImageUrl = string.IsNullOrEmpty(user.ProfileImageUrl)
                                    ? "/images/profile.png"
                                    : user.ProfileImageUrl,
                Background = user.Background,
                IsPublic = user.IsPublic,
                Links = user.Links.ToList()
            };

            return View(vm); // Views/Biolink/Settings.cshtml
        }

        // =====================================================
        // 🔹 POST Settings (update profile)
        // =====================================================
        [HttpPost("Settings")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Settings(ProfileSettingViewModel model, IFormFile? profileImage)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account", new { area = "Identity" });

            user.DisplayName = model.DisplayName;
            user.Bio = model.Bio;
            user.Background = model.Background;
            user.IsPublic = model.IsPublic;

            // ✅ upload foto profil
            if (profileImage != null && profileImage.Length > 0)
            {
                var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/profiles");
                if (!Directory.Exists(uploadDir))
                    Directory.CreateDirectory(uploadDir);

                // Hapus foto lama kalau ada
                if (!string.IsNullOrEmpty(user.ProfileImageUrl))
                {
                    var oldPath = Path.Combine(uploadDir, Path.GetFileName(user.ProfileImageUrl));
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(profileImage.FileName)}";
                var filePath = Path.Combine(uploadDir, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await profileImage.CopyToAsync(stream);
                }

                user.ProfileImageUrl = $"/images/profiles/{fileName}";
            }

            _context.Update(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Settings");
        }


        // =====================================================
        // 🔹 AddLink: /Biolink/AddLink
        // =====================================================
        [HttpPost("AddLink")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddLink(UserLink link)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account", new { area = "Identity" });

            link.UserId = user.Id;
            _context.UserLinks.Add(link);
            await _context.SaveChangesAsync();

            return RedirectToAction("Settings");
        }

        // =====================================================
        // 🔹 DeleteLink: /Biolink/DeleteLink
        // =====================================================
        [HttpPost("DeleteLink")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteLink(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account", new { area = "Identity" });

            var link = await _context.UserLinks
                .FirstOrDefaultAsync(l => l.Id == id && l.UserId == user.Id);

            if (link == null) return NotFound();

            _context.UserLinks.Remove(link);
            await _context.SaveChangesAsync();

            return RedirectToAction("Settings");
        }
    }
}
