namespace Lab7.Models
{
    public class RolesWithUsersViewModel
    {
        public string RoleId { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public List<string> Users { get; set; } = new();
    }
}
