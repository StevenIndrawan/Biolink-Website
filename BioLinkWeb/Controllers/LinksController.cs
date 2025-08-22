using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BioLinkWeb.Data;
using BioLinkWeb.Models;
using Microsoft.AspNetCore.Identity;

namespace BioLinkWeb.Controllers
{
    [Authorize]
    public class LinksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public LinksController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var links = _context.UserLinks.Where(l => l.UserId == user.Id).OrderBy(l => l.Order).ToList();
            return View(links);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(UserLink link)
        {
            var user = await _userManager.GetUserAsync(User);
            link.UserId = user.Id;
            _context.UserLinks.Add(link);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var link = _context.UserLinks.Find(id);
            if (link == null) return NotFound();
            return View(link);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserLink link)
        {
            _context.UserLinks.Update(link);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var link = _context.UserLinks.Find(id);
            if (link == null) return NotFound();
            return View(link);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var link = _context.UserLinks.Find(id);
            if (link != null)
            {
                _context.UserLinks.Remove(link);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
