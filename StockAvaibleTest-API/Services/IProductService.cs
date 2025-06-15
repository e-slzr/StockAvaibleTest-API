using StockAvaibleTest_API.Common;
using StockAvaibleTest_API.DTOs;

namespace StockAvaibleTest_API.Services
{
    public interface IProductService
    {
        Task<Result<IEnumerable<ProductDTO>>> GetAllProductsAsync();
        Task<Result<ProductDTO>> GetProductByIdAsync(int id);
        Task<Result<ProductDTO>> CreateProductAsync(CreateProductDTO productDto);
        Task<Result<ProductDTO>> UpdateProductAsync(int id, UpdateProductDTO productDto);
        Task<Result<bool>> DeleteProductAsync(int id);
        Task<Result<int>> GetAvailableStockAsync(int productId);
        Task<Result<bool>> HasSufficientStockAsync(int productId, int quantity);
        Task<Result<IEnumerable<ProductDTO>>> GetLowStockProductsAsync();
        Task<Result<ProductBoxLocationDTO>> GetProductLocationsAsync(int productId);
    }
}
