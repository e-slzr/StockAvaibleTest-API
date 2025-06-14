using StockAvaibleTest_API.Models;

namespace StockAvaibleTest_API.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsWithCategoryAsync();
        Task<Product?> GetProductWithCategoryAsync(int id);
        Task<int> GetAvailableStockAsync(int productId);
    }
}
