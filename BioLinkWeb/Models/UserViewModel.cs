namespace BioLinkWeb.Models
{
    public class UserViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string DisplayName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Bio { get; set; }
        public bool IsActive { get; set; }
        // Tambahkan alias biar view bisa pakai Username
        public string Username => UserName;
    }
}
