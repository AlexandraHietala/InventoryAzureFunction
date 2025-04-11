using System;
using System.ComponentModel.DataAnnotations;

namespace InventoryFunction.Models.System
{
    public class GenericIdUser
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string LastModifiedBy { get; set; }
    }
}
