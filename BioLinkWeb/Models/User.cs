using System.ComponentModel.DataAnnotations;

namespace BioLinkWeb.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        public string? DisplayName { get; set; }
        public string? Bio { get; set; }
        public string? ProfileImageUrl { get; set; }

        // Tambahkan properti yang dipakai Biolink
        public string? Background { get; set; }      // background profile / theme
        public bool IsPublic { get; set; } = true;  // apakah profil bisa dilihat publik
        public bool IsActive { get; set; } = true;  // status user aktif / nonaktif

        // Relasi ke Links
         public List<Link> Links { get; set; } = new();
    }
}
