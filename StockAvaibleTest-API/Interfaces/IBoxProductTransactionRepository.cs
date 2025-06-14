using StockAvaibleTest_API.Models;

namespace StockAvaibleTest_API.Interfaces
{
    public interface IBoxProductTransactionRepository : IGenericRepository<BoxProductTransaction>
    {
        Task<IEnumerable<BoxProductTransaction>> GetTransactionsByBoxAsync(int boxId);
        Task<IEnumerable<BoxProductTransaction>> GetTransactionsByProductAsync(int productId);
        Task<bool> ValidateStockAvailabilityAsync(int boxId, int productId, int quantity);
    }
}
