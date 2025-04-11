using System.ComponentModel.DataAnnotations;

namespace InventoryFunction.Models.DTOs
{
    public class AuthDto
    {
        [Required]
        public string PASS_SALT { get; set; }

        [Required]
        public string PASS_HASH { get; set; }

        public int? ROLE_ID { get; set; }
    }
}
