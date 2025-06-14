namespace StockAvaibleTest_API.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        ICategoryRepository Categories { get; }
        IBoxRepository Boxes { get; }
        IBoxProductTransactionRepository BoxProductTransactions { get; }
        
        Task<int> CompleteAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
