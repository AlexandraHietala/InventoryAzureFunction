using System;
using System.ComponentModel.DataAnnotations;

namespace InventoryFunction.Models.DTOs
{
    public class SeriesDto
    {
        [Required]
        public int SERIES_ID { get; set; }

        [Required]
        public string SERIES_NAME { get; set; }

        public string SERIES_DESCRIPTION { get; set; }

        [Required]
        public string SERIES_CREATED_BY { get; set; }

        [Required]
        public DateTime SERIES_CREATED_DATE { get; set; }

        public string SERIES_LAST_MODIFIED_BY { get; set; }

        public DateTime SERIES_LAST_MODIFIED_DATE { get; set; }
    }
}
