namespace StockAvaibleTest_API.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int MinimumStock { get; set; }
        public string Unit { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime? LastTransactionDate { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int AvailableStock { get; set; }
    }

    public class CreateProductDTO
    {
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int MinimumStock { get; set; }
        public string Unit { get; set; } = string.Empty;
        public int CategoryId { get; set; }
    }

    public class UpdateProductDTO
    {
        public string Description { get; set; } = string.Empty;
        public int MinimumStock { get; set; }
        public string Unit { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int CategoryId { get; set; }
    }
}
