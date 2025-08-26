using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using BioLinkWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace BioLinkWeb.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Username wajib diisi")]
            public string UserName { get; set; }

            [Required(ErrorMessage = "Email wajib diisi")]
            [EmailAddress]
            public string Email { get; set; }

            [Required(ErrorMessage = "Password wajib diisi")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Password dan konfirmasi tidak sama")]
            public string ConfirmPassword { get; set; }

            public string? DisplayName { get; set; }
            public string? Bio { get; set; }
            public bool IsPublic { get; set; } = true;
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
                var user = new ApplicationUser
                {
                    UserName = Input.UserName,
                    Email = Input.Email,
                    DisplayName = Input.DisplayName,
                    Bio = Input.Bio,
                    IsPublic = Input.IsPublic,
                    EmailConfirmed = true // langsung confirmed
                };

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User {UserName} berhasil registrasi.", user.UserName);

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }

                // tampilkan error dari Identity ke UI
                foreach (var error in result.Errors)
                {
                    _logger.LogWarning("Registrasi gagal: {Error}", error.Description);
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // kalau gagal, balik ke form lagi
            return Page();
        }
    }
}
