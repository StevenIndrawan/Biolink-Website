using System.ComponentModel.DataAnnotations;

namespace BioLinkWeb.Models
{
    public class UserViewModel
    {
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "Display Name wajib diisi")]
        public string DisplayName { get; set; }

        [Required(ErrorMessage = "Username wajib diisi")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email wajib diisi")]
        [EmailAddress(ErrorMessage = "Format email tidak valid")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }   // Required hanya di Create

        public string Bio { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsPublic { get; set; } = true;

        // ✅ Jangan Required
        public string? ProfileImageUrl { get; set; } = null;
    }
}
