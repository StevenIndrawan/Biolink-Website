using BioLinkWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BioLinkWeb.Controllers
{
    public class DashboardController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // GET: Dashboard
        public IActionResult Index()
        {
            var users = _userManager.Users.Select(u => new UserViewModel
            {
                Id = u.Id,
                UserName = u.UserName,
                DisplayName = u.DisplayName,
                Email = u.Email,
                Bio = u.Bio,
                IsActive = u.IsActive
            }).ToList();

            return View(users);
        }

        // POST: Update User (from popup modal)
        [HttpPost]
        public async Task<IActionResult> UpdateUser(UserViewModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index");

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user != null)
            {
                user.DisplayName = model.DisplayName;
                user.Email = model.Email;
                user.UserName = model.UserName; // optional, jika ingin bisa ubah username
                user.Bio = model.Bio;
                user.IsActive = model.IsActive;

                await _userManager.UpdateAsync(user);
            }

            return RedirectToAction("Index");
        }
    }
}
