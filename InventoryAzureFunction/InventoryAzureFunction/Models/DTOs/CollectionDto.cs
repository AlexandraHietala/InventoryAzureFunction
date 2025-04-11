using System;
using System.ComponentModel.DataAnnotations;

namespace SEInventoryCollection.Models.DTOs
{
    public class CollectionDto
    {
        [Required]
        public int COLLECTION_ID { get; set; }

        [Required]
        public string COLLECTION_NAME { get; set; }

        public string COLLECTION_DESCRIPTION { get; set; }

        [Required]
        public string COLLECTION_CREATED_BY { get; set; }

        [Required]
        public DateTime COLLECTION_CREATED_DATE { get; set; }

        public string COLLECTION_LAST_MODIFIED_BY { get; set; }

        public DateTime COLLECTION_LAST_MODIFIED_DATE { get; set; }
    }
}
