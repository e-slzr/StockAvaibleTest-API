using Microsoft.EntityFrameworkCore;
using StockAvaibleTest_API.Data;
using StockAvaibleTest_API.Interfaces;
using StockAvaibleTest_API.Models;

namespace StockAvaibleTest_API.Repositories
{
    public class BoxRepository : GenericRepository<Box>, IBoxRepository
    {
        public BoxRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Box>> GetBoxesWithTransactionsAsync()
        {
            return await _context.Boxes
                .Include(b => b.Transactions)
                .ToListAsync();
        }

        public async Task<Box?> GetBoxWithTransactionsAsync(int id)
        {
            return await _context.Boxes
                .Include(b => b.Transactions)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<int> GetAvailableProductQuantityAsync(int boxId, int productId)
        {
            var transactions = await _context.BoxProductTransactions
                .Where(t => t.BoxId == boxId && t.ProductId == productId)
                .ToListAsync();

            int inStock = transactions.Where(t => t.Type == "IN").Sum(t => t.Quantity);
            int outStock = transactions.Where(t => t.Type == "OUT").Sum(t => t.Quantity);

            return inStock - outStock;
        }
    }
}
