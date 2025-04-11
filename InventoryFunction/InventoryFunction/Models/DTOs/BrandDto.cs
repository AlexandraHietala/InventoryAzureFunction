using System;
using System.ComponentModel.DataAnnotations;

namespace InventoryFunction.Models.DTOs
{
    public class BrandDto
    {
        [Required]
        public int BRAND_ID { get; set; }

        [Required]
        public string BRAND_NAME { get; set; }

        public string BRAND_DESCRIPTION { get; set; }

        [Required]
        public string BRAND_CREATED_BY { get; set; }

        [Required]
        public DateTime BRAND_CREATED_DATE { get; set; }

        public string BRAND_LAST_MODIFIED_BY { get; set; }

        public DateTime BRAND_LAST_MODIFIED_DATE { get; set; }
    }
}
