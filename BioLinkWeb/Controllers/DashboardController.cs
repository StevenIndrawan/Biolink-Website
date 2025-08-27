using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BioLinkWeb.Data;
using BioLinkWeb.Models;
using System.Threading.Tasks;
using System.Linq;

namespace BioLinkWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public DashboardController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users
                .Select(u => new UserViewModel
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    DisplayName = u.DisplayName,
                    Email = u.Email,
                    Bio = u.Bio,
                    IsActive = u.IsActive,   // ✅ benar
                    IsPublic = u.IsPublic    // ✅ jangan lupa
                })
                .ToListAsync();

            return View(users);
        }

        private async Task<IActionResult> ReloadUserTable()
        {
            var users = await _context.Users
                .Select(u => new UserViewModel
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    DisplayName = u.DisplayName,
                    Email = u.Email,
                    Bio = u.Bio,
                    IsActive = u.IsActive,
                    IsPublic = u.IsPublic
                })
                .ToListAsync();

            return PartialView("_UserTablePartial", users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(UserViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Password))
            {
                ModelState.AddModelError("Password", "Password wajib diisi saat membuat user");
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                DisplayName = model.DisplayName,
                Email = model.Email,
                Bio = model.Bio,
                IsActive = model.IsActive,
                IsPublic = model.IsPublic,
                ProfileImageUrl = model.ProfileImageUrl // boleh null
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return await ReloadUserTable();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUser(UserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null) return NotFound();

            user.DisplayName = model.DisplayName;
            user.Email = model.Email;
            user.Bio = model.Bio;
            user.IsActive = model.IsActive;   // ✅ benar
            user.IsPublic = model.IsPublic;   // ✅ benar

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(result.Errors);

            if (!string.IsNullOrEmpty(model.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                await _userManager.ResetPasswordAsync(user, token, model.Password);
            }

            return await ReloadUserTable();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return await ReloadUserTable();
        }
    }
}
