using System;
using System.ComponentModel.DataAnnotations;

namespace InventoryFunction.Models.DTOs
{
    public class ItemExpandedDto
    {
        [Required]
        public int ID { get; set; }

        [Required]
        public CollectionDto COLLECTION_ID { get; set; }

        [Required]
        public string STATUS { get; set; }

        [Required]
        public string TYPE { get; set; }

        public BrandDto Brand { get; set; }

        public SeriesDto Series { get; set; }

        public string NAME { get; set; }

        public string DESCRIPTION { get; set; }

        [Required]
        public string FORMAT { get; set; }

        [Required]
        public string SIZE { get; set; }

        public int? YEAR { get; set; }

        public string PHOTO { get; set; }

        [Required]
        public string CREATED_BY { get; set; }

        [Required]
        public DateTime CREATED_DATE { get; set; }

        public string LAST_MODIFIED_BY { get; set; }

        public DateTime LAST_MODIFIED_DATE { get; set; }
    }
}
