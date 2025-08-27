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
                    IsActive = u.IsPublic
                })
                .ToListAsync();

            return View(users); 
        }

        [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> CreateUser(UserViewModel model)
{
    if (!ModelState.IsValid)
    {
        var usersInvalid = await GetUsers();
        return PartialView("_UserTablePartial", usersInvalid);
    }

    var user = new ApplicationUser
    {
        UserName = model.UserName,
        DisplayName = model.DisplayName,
        Email = model.Email,
        Bio = model.Bio,
        IsPublic = model.IsActive
    };

    var result = await _userManager.CreateAsync(user, model.Password);
    if (!result.Succeeded)
    {
        TempData["Error"] = "Gagal membuat user baru.";
        var usersError = await GetUsers();
        return PartialView("_UserTablePartial", usersError);
    }

    var users = await GetUsers();
    return PartialView("_UserTablePartial", users);
}

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateUser(UserViewModel model)
    {
        var user = await _userManager.FindByIdAsync(model.Id);
        if (user == null)
        {
            TempData["Error"] = "User tidak ditemukan.";
            var usersError = await GetUsers();
            return PartialView("_UserTablePartial", usersError);
        }

        user.DisplayName = model.DisplayName;
        user.Email = model.Email;
        user.Bio = model.Bio;
        user.IsPublic = model.IsActive;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            TempData["Error"] = "Gagal memperbarui user.";
            var usersError = await GetUsers();
            return PartialView("_UserTablePartial", usersError);
        }

        if (!string.IsNullOrEmpty(model.Password))
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _userManager.ResetPasswordAsync(user, token, model.Password);
        }

        var users = await GetUsers();
        return PartialView("_UserTablePartial", users);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            TempData["Error"] = "User tidak ditemukan.";
            var usersError = await GetUsers();
            return PartialView("_UserTablePartial", usersError);
        }

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            TempData["Error"] = "Gagal menghapus user.";
            var usersError = await GetUsers();
            return PartialView("_UserTablePartial", usersError);
        }

        var users = await GetUsers();
        return PartialView("_UserTablePartial", users);
    }

    /// helper
    private async Task<List<UserViewModel>> GetUsers()
    {
        return await _context.Users
            .Select(u => new UserViewModel
            {
                Id = u.Id,
                UserName = u.UserName,
                DisplayName = u.DisplayName,
                Email = u.Email,
                Bio = u.Bio,
                IsActive = u.IsPublic
            })
            .ToListAsync();
    }

    }
}
