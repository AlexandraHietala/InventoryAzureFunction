using System.ComponentModel.DataAnnotations;

namespace InventoryFunction.Models.System
{ 
    public class ValidationFailure
    {
        [Required]
        public int Code { get; set; }

        [Required]
        public string Message { get; set; }

    }
}
