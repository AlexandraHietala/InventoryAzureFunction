using SEInventoryCollection.Models.System;
using System.ComponentModel.DataAnnotations;

namespace SEInventoryCollection.Models.Classes
{
    public class Brand : Base
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string BrandName { get; set; }

        public string Description { get; set; }

    }
}
