using Microsoft.AspNetCore.Mvc;
using BioLinkWeb.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BioLinkWeb.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Ambil semua user dari database
            var users = await _context.Users.ToListAsync();
            return View(users);
        }
    }
}
