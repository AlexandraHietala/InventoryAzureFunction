using System;
using System.ComponentModel.DataAnnotations;

namespace SEInventoryCollection.Models.System
{
    public class Base
    {
        [Required]
        public string CreatedBy { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime LastModifiedDate { get; set; }
    }
}
