using System.ComponentModel.DataAnnotations;

namespace StockAvaibleTest_API.Models
{
    /// <summary>
    /// Represents a product in the inventory system
    /// </summary>
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Description { get; set; } = string.Empty;

        public int MinimumStock { get; set; }

        [Required]
        [StringLength(50)]
        public string Unit { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public int CategoryId { get; set; }

        public DateTime? LastTransactionDate { get; set; }

        // Navigation properties
        public virtual Category Category { get; set; } = null!;
        public virtual ICollection<BoxProductTransaction> Transactions { get; set; } = new List<BoxProductTransaction>();
    }
}
