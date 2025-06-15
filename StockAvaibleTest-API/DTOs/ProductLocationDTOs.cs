namespace StockAvaibleTest_API.DTOs
{
    public class ProductBoxLocationDTO
    {
        public int ProductId { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<BoxStockDTO> Boxes { get; set; } = new List<BoxStockDTO>();
    }

    public class BoxStockDTO
    {
        public int BoxId { get; set; }
        public string BoxCode { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public int CurrentStock { get; set; }
        public DateTime? LastTransactionDate { get; set; }
    }
}
