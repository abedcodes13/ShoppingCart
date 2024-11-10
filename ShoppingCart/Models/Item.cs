using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCart.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public required string Name {  get; set; }
    
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        public string? Description { get; set; }

        [Required]
        public string? Category { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Only positive numbers or zero is allowed")]
        public int Quantity { get; set; }

        public string? CreatedBy { get; set; } // Add this field

    }
}
