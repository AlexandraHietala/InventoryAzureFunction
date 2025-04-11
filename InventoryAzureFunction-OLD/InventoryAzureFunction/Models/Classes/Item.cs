using SEInventoryCollection.Models.System;
using System.ComponentModel.DataAnnotations;

namespace SEInventoryCollection.Models.Classes
{
    public class Item : Base
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int CollectionId { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public string Type { get; set; }

        public int? BrandId { get; set; }

        public int? SeriesId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public string Format { get; set; }

        [Required]
        public string Size { get; set; }

        public int? Year { get; set; }

        public string Photo { get; set; }

        // TODO: Add UPC/Barcode, Sku, storage location
    }
}
