namespace StockAvaibleTest_API.DTOs
{
    public class TransactionDTO
    {
        public int Id { get; set; }
        public int BoxId { get; set; }
        public string BoxCode { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string Type { get; set; } = string.Empty;
        public DateTime TransactionDate { get; set; }
    }

    public class CreateTransactionDTO
    {
        public int BoxId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Type { get; set; } = string.Empty;
    }
}
