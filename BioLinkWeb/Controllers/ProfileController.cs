using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BioLinkWeb.Data;
using BioLinkWeb.Models;
using Microsoft.AspNetCore.Identity;

namespace BioLinkWeb.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ProfileController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var profile = _context.UserProfiles.FirstOrDefault(p => p.UserId == user.Id);

            if (profile == null)
            {
                profile = new UserProfile { UserId = user.Id };
                _context.UserProfiles.Add(profile);
                await _context.SaveChangesAsync();
            }

            return View(profile);
        }

        [HttpPost]
        public async Task<IActionResult> Save(UserProfile profile)
        {
            var user = await _userManager.GetUserAsync(User);
            var existing = _context.UserProfiles.FirstOrDefault(p => p.UserId == user.Id);

            if (existing != null)
            {
                existing.DisplayName = profile.DisplayName;
                existing.Bio = profile.Bio;
                existing.ProfileImageUrl = profile.ProfileImageUrl;
                _context.Update(existing);
            }
            else
            {
                profile.UserId = user.Id;
                _context.Add(profile);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
