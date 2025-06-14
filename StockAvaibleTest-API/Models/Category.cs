using System.ComponentModel.DataAnnotations;

namespace StockAvaibleTest_API.Models
{
    /// <summary>
    /// Represents a product category in the inventory system
    /// </summary>
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation property
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
