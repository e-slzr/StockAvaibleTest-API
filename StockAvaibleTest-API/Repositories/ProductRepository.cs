using Microsoft.EntityFrameworkCore;
using StockAvaibleTest_API.Data;
using StockAvaibleTest_API.Interfaces;
using StockAvaibleTest_API.Models;

namespace StockAvaibleTest_API.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Product>> GetProductsWithCategoryAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .ToListAsync();
        }

        public async Task<Product?> GetProductWithCategoryAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<int> GetAvailableStockAsync(int productId)
        {
            var transactions = await _context.BoxProductTransactions
                .Where(t => t.ProductId == productId)
                .ToListAsync();

            int inStock = transactions.Where(t => t.Type == "IN").Sum(t => t.Quantity);
            int outStock = transactions.Where(t => t.Type == "OUT").Sum(t => t.Quantity);

            return inStock - outStock;
        }
    }
}
