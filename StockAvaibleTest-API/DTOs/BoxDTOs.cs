namespace StockAvaibleTest_API.DTOs
{
    public class BoxDTO
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime? LastOperationDate { get; set; }
    }

    public class BoxDetailDTO : BoxDTO
    {
        public IEnumerable<BoxProductQuantityDTO> Products { get; set; } = new List<BoxProductQuantityDTO>();
    }

    public class BoxProductQuantityDTO
    {
        public int ProductId { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public int AvailableQuantity { get; set; }
    }

    public class CreateBoxDTO
    {
        public string Code { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
    }

    public class UpdateBoxDTO
    {
        public string Location { get; set; } = string.Empty;
    }
}
