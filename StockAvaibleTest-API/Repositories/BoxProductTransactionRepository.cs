using Microsoft.EntityFrameworkCore;
using StockAvaibleTest_API.Data;
using StockAvaibleTest_API.Interfaces;
using StockAvaibleTest_API.Models;

namespace StockAvaibleTest_API.Repositories
{
    public class BoxProductTransactionRepository : GenericRepository<BoxProductTransaction>, IBoxProductTransactionRepository
    {
        public BoxProductTransactionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<BoxProductTransaction?> GetByIdAsync(int id)
        {
            return await _context.BoxProductTransactions
                .Include(t => t.Box)
                .Include(t => t.Product)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public override async Task<IEnumerable<BoxProductTransaction>> GetAllAsync()
        {
            return await _context.BoxProductTransactions
                .Include(t => t.Box)
                .Include(t => t.Product)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<BoxProductTransaction>> GetTransactionsByBoxAsync(int boxId)
        {
            return await _context.BoxProductTransactions
                .Include(t => t.Box)
                .Include(t => t.Product)
                .Where(t => t.BoxId == boxId)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<BoxProductTransaction>> GetTransactionsByProductAsync(int productId)
        {
            return await _context.BoxProductTransactions
                .Include(t => t.Box)
                .Include(t => t.Product)
                .Where(t => t.ProductId == productId)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

        public async Task<bool> ValidateStockAvailabilityAsync(int boxId, int productId, int quantity)
        {
            var transactions = await _context.BoxProductTransactions
                .Where(t => t.BoxId == boxId && t.ProductId == productId)
                .ToListAsync();

            int inStock = transactions.Where(t => t.Type == "IN").Sum(t => t.Quantity);
            int outStock = transactions.Where(t => t.Type == "OUT").Sum(t => t.Quantity);
            int available = inStock - outStock;

            return available >= quantity;
        }
    }
}
