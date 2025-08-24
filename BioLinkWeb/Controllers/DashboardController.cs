using Microsoft.AspNetCore.Mvc;
using BioLinkWeb.Data;
using BioLinkWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BioLinkWeb.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // READ (List)
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users
                .Select(u => new UserViewModel
                {
                    Id = u.Id,
                    DisplayName = u.DisplayName,
                    UserName = u.UserName,
                    Email = u.Email,
                    Bio = u.Bio,
                    IsActive = u.IsActive
                }).ToListAsync();

            return View(users);
        }

        // CREATE (GET)
        public IActionResult Create()
        {
            return View();
        }

        // CREATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    DisplayName = model.DisplayName,
                    Bio = model.Bio,
                    IsActive = model.IsActive
                };

                await _userManager.CreateAsync(user, "Password123!"); // default password
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // EDIT (GET)
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            var model = new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                DisplayName = user.DisplayName,
                Bio = user.Bio,
                IsActive = user.IsActive
            };

            return View(model);
        }

        // EDIT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FindAsync(model.Id);
                if (user == null) return NotFound();

                user.DisplayName = model.DisplayName;
                user.Email = model.Email;
                user.Bio = model.Bio;
                user.IsActive = model.IsActive;

                _context.Update(user);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // DELETE (GET)
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            var model = new UserViewModel
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Email = user.Email
            };

            return View(model);
        }

        // DELETE (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
