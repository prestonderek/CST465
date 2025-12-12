using System.ComponentModel.DataAnnotations;

namespace Lab7.Models
{
    public class CreateRoleViewModel
    {
        [Required]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; } = string.Empty;
    }
}
