using System.ComponentModel.DataAnnotations;

namespace StockAvaibleTest_API.Models
{
    /// <summary>
    /// Represents a transaction between a box and a product in the inventory system
    /// </summary>
    public class BoxProductTransaction
    {
        public int Id { get; set; }

        public int BoxId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        [Required]
        [StringLength(10)]
        public string Type { get; set; } = string.Empty; // "IN" or "OUT"

        public DateTime TransactionDate { get; set; }

        // Navigation properties
        public virtual Box Box { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
