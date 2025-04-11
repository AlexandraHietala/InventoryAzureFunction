using SEInventoryCollection.Models.System;
using System.ComponentModel.DataAnnotations;

namespace SEInventoryCollection.Models.Classes
{
    public class Collection : Base
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string CollectionName { get; set; }

        public string Description { get; set; }
    }
}
