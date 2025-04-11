using System.ComponentModel.DataAnnotations;

namespace InventoryFunction.Models.Classes
{
    public class Role
    {
        [Required]
        public int Id { get; set; }

        public string Description { get; set; }
    }
}
