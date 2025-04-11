using SEInventoryCollection.Models.System;
using System.ComponentModel.DataAnnotations;

namespace SEInventoryCollection.Models.Classes
{
    public class ItemExpanded : Base
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public Collection CollectionId { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public string Type { get; set; }

        public Brand Brand { get; set; }

        public Series Series { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public string Format { get; set; }

        [Required]
        public string Size { get; set; }

        public int? Year { get; set; }

        public string Photo { get; set; }
    }
}
