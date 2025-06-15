using System.ComponentModel.DataAnnotations;

namespace StockAvaibleTest_API.Models
{
    /// <summary>
    /// Represents a storage box in the inventory system
    /// </summary>
    public class Box
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Location { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue)]
        public int TotalCapacity { get; set; }

        public DateTime? LastOperationDate { get; set; }

        // Navigation property
        public virtual ICollection<BoxProductTransaction> Transactions { get; set; } = new List<BoxProductTransaction>();
    }
}
