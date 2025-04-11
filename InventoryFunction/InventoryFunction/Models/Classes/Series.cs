using InventoryFunction.Models.System;
using System.ComponentModel.DataAnnotations;

namespace InventoryFunction.Models.Classes
{
    public class Series : Base
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string SeriesName { get; set; }

        public string Description { get; set; }
    }
}
