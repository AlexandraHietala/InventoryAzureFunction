using System;
using System.ComponentModel.DataAnnotations;

namespace SEInventoryCollection.Models.DTOs
{
    public class ItemCommentDto
    {
        [Required]
        public int COMMENT_ID { get; set; }

        [Required]
        public int ITEM_ID { get; set; }

        [Required]
        public string COMMENT { get; set; }

        [Required]
        public string COMMENT_CREATED_BY { get; set; }

        [Required]
        public DateTime COMMENT_CREATED_DATE { get; set; }

        public string COMMENT_LAST_MODIFIED_BY { get; set; }

        public DateTime COMMENT_LAST_MODIFIED_DATE { get; set; }
    }
}
