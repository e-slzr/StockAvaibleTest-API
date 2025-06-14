using StockAvaibleTest_API.Models;

namespace StockAvaibleTest_API.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<IEnumerable<Category>> GetCategoriesWithProductsAsync();
        Task<Category?> GetCategoryWithProductsAsync(int id);
    }
}
