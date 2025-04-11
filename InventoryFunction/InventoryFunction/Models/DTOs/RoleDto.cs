using System.ComponentModel.DataAnnotations;

namespace InventoryFunction.Models.DTOs
{
    public class RoleDto
    {
        [Required]
        public int ROLE_ID { get; set; }

        public string ROLE_DESCRIPTION { get; set; }
    }
}
