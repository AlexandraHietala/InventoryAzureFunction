using InventoryFunction.Models.System;
using System.ComponentModel.DataAnnotations;

namespace InventoryFunction.Models.Classes
{
    public class User : Base
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string PassSalt { get; set; }

        [Required]
        public string PassHash { get; set; }

        public int? RoleId { get; set; }
    }
}
