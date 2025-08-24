using Microsoft.AspNetCore.Mvc;

namespace BioLinkWeb.Controllers
{
    public class AuthController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View(); // Views/Auth/Login.cshtml
        }

        [HttpPost]
        public IActionResult Login(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                ViewBag.Error = "Username wajib diisi!";
                return View();
            }

            // Simpan ke Session
            HttpContext.Session.SetString("Username", username);

            return RedirectToAction("Profile", "Auth");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public IActionResult Profile()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login");
            }

            // arahkan ke profile user (Biolink)
            return RedirectToAction("Index", "Biolink", new { username });
        }
    }
}
