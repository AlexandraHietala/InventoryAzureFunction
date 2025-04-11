using SEInventoryCollection.Models.System;
using System.ComponentModel.DataAnnotations;

namespace SEInventoryCollection.Models.Classes
{
    public class ItemComment : Base
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Required]
        public string Comment { get; set; }
    }
}
