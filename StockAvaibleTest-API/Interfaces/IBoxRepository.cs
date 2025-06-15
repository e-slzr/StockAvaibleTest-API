using StockAvaibleTest_API.Models;

namespace StockAvaibleTest_API.Interfaces
{
    public interface IBoxRepository : IGenericRepository<Box>
    {
        Task<IEnumerable<Box>> GetBoxesWithTransactionsAsync();
        Task<Box?> GetBoxWithTransactionsAsync(int id);
        Task<int> GetAvailableProductQuantityAsync(int boxId, int productId);
        Task<int> GetTotalProductsInBoxAsync(int boxId);
    }
}
