using System.ComponentModel.DataAnnotations;

namespace StockAvaibleTest_API.DTOs
{
    public class BoxDTO
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public int TotalCapacity { get; set; }
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

        [Required(ErrorMessage = "La capacidad total es requerida")]
        [Range(1, int.MaxValue, ErrorMessage = "La capacidad total debe ser mayor a 0")]
        public int TotalCapacity { get; set; }
    }

    public class UpdateBoxDTO
    {
        public string Location { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "La capacidad total debe ser mayor a 0")]
        public int? TotalCapacity { get; set; }
    }
}
