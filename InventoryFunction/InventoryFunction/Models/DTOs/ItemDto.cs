using System;
using System.ComponentModel.DataAnnotations;

namespace InventoryFunction.Models.DTOs
{
    public class ItemDto
    {
        [Required]
        public int ID { get; set; }

        [Required]
        public int COLLECTION_ID { get; set; }

        [Required]
        public string STATUS { get; set; } 

        [Required]
        public string TYPE { get; set; }

        public int? BRAND_ID { get; set; }

        public int? SERIES_ID { get; set; }

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
