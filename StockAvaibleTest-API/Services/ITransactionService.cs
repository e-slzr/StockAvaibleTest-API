using StockAvaibleTest_API.Common;
using StockAvaibleTest_API.DTOs;

namespace StockAvaibleTest_API.Services
{
    public interface ITransactionService
    {
        Task<Result<IEnumerable<TransactionDTO>>> GetAllTransactionsAsync();
        Task<Result<TransactionDTO>> GetTransactionByIdAsync(int id);
        Task<Result<TransactionDTO>> CreateTransactionAsync(CreateTransactionDTO transactionDto);
        Task<Result<IEnumerable<TransactionDTO>>> GetTransactionsByBoxAsync(int boxId);
        Task<Result<IEnumerable<TransactionDTO>>> GetTransactionsByProductAsync(int productId);
    }
}
