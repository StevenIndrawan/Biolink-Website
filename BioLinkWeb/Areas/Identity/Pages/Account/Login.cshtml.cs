using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using BioLinkWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioLinkWeb.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Username atau Email wajib diisi")]
            [Display(Name = "Username atau Email")]
            public string UserNameOrEmail { get; set; }

            [Required(ErrorMessage = "Password wajib diisi")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Ingat Saya")]
            public bool RememberMe { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/");
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                // cari user berdasarkan username
                var user = await _userManager.FindByNameAsync(Input.UserNameOrEmail);

                // kalau tidak ketemu, coba cari berdasarkan email
                if (user == null)
                    user = await _userManager.FindByEmailAsync(Input.UserNameOrEmail);

                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(
                        user.UserName, Input.Password, Input.RememberMe, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        // setelah login, langsung redirect ke halaman biolink username
                        return LocalRedirect($"/{user.UserName}");
                    }
                }

                ModelState.AddModelError(string.Empty, "Login gagal. Periksa kembali username/email dan password.");
            }

            // jika gagal, tetap di halaman login
            return Page();
        }
    }
}
