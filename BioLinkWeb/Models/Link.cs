using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BioLinkWeb.Models
{
    public class Link
{
    public int Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string? Icon { get; set; }

    // 🔥 Tambahkan ini supaya bisa mengatur urutan link
    public int Order { get; set; }

    // Relasi balik ke User
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
}
