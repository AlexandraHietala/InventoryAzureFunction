using System.ComponentModel.DataAnnotations;

namespace InventoryFunction.Models.Classes
{
    public class Auth
    {
        [Required]
        public string PassSalt { get; set; }

        [Required]
        public string PassHash { get; set; }

        public int? RoleId { get; set; }
    }
}
