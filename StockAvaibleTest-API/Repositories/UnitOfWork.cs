using Microsoft.EntityFrameworkCore.Storage;
using StockAvaibleTest_API.Data;
using StockAvaibleTest_API.Interfaces;

namespace StockAvaibleTest_API.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;
        private IProductRepository? _productRepository;
        private ICategoryRepository? _categoryRepository;
        private IBoxRepository? _boxRepository;
        private IBoxProductTransactionRepository? _boxProductTransactionRepository;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IProductRepository Products => 
            _productRepository ??= new ProductRepository(_context);

        public ICategoryRepository Categories =>
            _categoryRepository ??= new CategoryRepository(_context);

        public IBoxRepository Boxes =>
            _boxRepository ??= new BoxRepository(_context);

        public IBoxProductTransactionRepository BoxProductTransactions =>
            _boxProductTransactionRepository ??= new BoxProductTransactionRepository(_context);

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                }
            }
            catch
            {
                if (_transaction != null)
                {
                    await _transaction.RollbackAsync();
                }
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
