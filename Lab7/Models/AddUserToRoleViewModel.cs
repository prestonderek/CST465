using Microsoft.AspNetCore.Identity;

namespace Lab7.Models
{
    public class AddUserToRoleViewModel
    {
        public string? SelectedUserId { get; set; }
        public string? SelectedRoleName { get; set; }

        public List<IdentityUser> Users { get; set; } = new();
        public List<IdentityRole> Roles { get; set; } = new();
    }
}

