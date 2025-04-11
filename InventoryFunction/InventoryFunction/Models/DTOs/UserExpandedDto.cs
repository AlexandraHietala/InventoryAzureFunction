using System;
using System.ComponentModel.DataAnnotations;

namespace InventoryFunction.Models.DTOs
{
    public class UserExpandedDto
    {
        [Required]
        public int ID { get; set; }

        [Required]
        public string NAME { get; set; }

        [Required]
        public string PASS_SALT { get; set; }

        [Required]
        public string PASS_HASH { get; set; }

        public RoleDto ROLE { get; set; }

        [Required]
        public string CREATED_BY { get; set; }

        [Required]
        public DateTime CREATED_DATE { get; set; }

        public string LAST_MODIFIED_BY { get; set; }

        public DateTime LAST_MODIFIED_DATE { get; set; }
    }
}
