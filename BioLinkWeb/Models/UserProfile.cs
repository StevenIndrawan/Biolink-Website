using System.ComponentModel.DataAnnotations;

namespace BioLinkWeb.Models
{
    public class UserProfile
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }   // navigation ke ApplicationUser

        [Required]
        [MaxLength(50)]
        public string DisplayName { get; set; }

        [MaxLength(200)]
        public string? Bio { get; set; }

        public bool IsPublic { get; set; } = true;
    }
}
