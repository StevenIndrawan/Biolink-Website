using System.ComponentModel.DataAnnotations;

namespace BioLinkWeb.Models
{
    public class UserLink
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Title { get; set; } = string.Empty;
        public ApplicationUser User { get; set; }
        [Required, Url]
        public string Url { get; set; } = string.Empty;

        public int Order { get; set; } = 0;
    }
}
