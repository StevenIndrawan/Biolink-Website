using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BioLinkWeb.Models
{
    public class Link
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Url { get; set; } = string.Empty;

        public int Order { get; set; } = 0;

        // 🔹 Tambahkan property Icon (opsional)
        public string? Icon { get; set; }

        // Foreign Key ke ApplicationUser
        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }
}
