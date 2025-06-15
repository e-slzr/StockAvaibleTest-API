using AutoMapper;
using StockAvaibleTest_API.Common;
using StockAvaibleTest_API.DTOs;
using StockAvaibleTest_API.Interfaces;
using StockAvaibleTest_API.Models;

namespace StockAvaibleTest_API.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TransactionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<TransactionDTO>>> GetAllTransactionsAsync()
        {
            try
            {
                var transactions = await _unitOfWork.BoxProductTransactions.GetAllAsync();
                var transactionDtos = _mapper.Map<IEnumerable<TransactionDTO>>(transactions);
                return Result<IEnumerable<TransactionDTO>>.Success(transactionDtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<TransactionDTO>>.Failure($"Error al obtener las transacciones: {ex.Message}");
            }
        }

        public async Task<Result<TransactionDTO>> GetTransactionByIdAsync(int id)
        {
            try
            {
                var transaction = await _unitOfWork.BoxProductTransactions.GetByIdAsync(id);
                if (transaction == null)
                    return Result<TransactionDTO>.Failure($"No se encontró la transacción con ID: {id}");

                var transactionDto = _mapper.Map<TransactionDTO>(transaction);
                return Result<TransactionDTO>.Success(transactionDto);
            }
            catch (Exception ex)
            {
                return Result<TransactionDTO>.Failure($"Error al obtener la transacción: {ex.Message}");
            }
        }

        public async Task<Result<TransactionDTO>> CreateTransactionAsync(CreateTransactionDTO transactionDto)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                // Validar que existan la caja y el producto
                var box = await _unitOfWork.Boxes.GetByIdAsync(transactionDto.BoxId);
                if (box == null)
                    return Result<TransactionDTO>.Failure($"No existe la caja con ID: {transactionDto.BoxId}");

                var product = await _unitOfWork.Products.GetByIdAsync(transactionDto.ProductId);
                if (product == null)
                    return Result<TransactionDTO>.Failure($"No existe el producto con ID: {transactionDto.ProductId}");

                if (!product.IsActive)
                    return Result<TransactionDTO>.Failure($"El producto con ID: {transactionDto.ProductId} está inactivo");

                // Validar stock suficiente para transacciones de salida
                if (transactionDto.Type == "OUT")
                {
                    var availableInBox = await _unitOfWork.Boxes
                        .GetAvailableProductQuantityAsync(transactionDto.BoxId, transactionDto.ProductId);

                    if (availableInBox < transactionDto.Quantity)
                        return Result<TransactionDTO>.Failure("No hay suficiente stock en la caja para realizar la salida");
                }
                else if (transactionDto.Type == "IN")
                {
                    // Calcular cantidad actual en la caja
                    var currentBoxQuantity = await _unitOfWork.Boxes
                        .GetTotalProductsInBoxAsync(transactionDto.BoxId);

                    // Validar capacidad de la caja
                    if (currentBoxQuantity + transactionDto.Quantity > box.TotalCapacity)
                    {
                        var availableCapacity = box.TotalCapacity - currentBoxQuantity;
                        return Result<TransactionDTO>.Failure(
                            $"La cantidad excede la capacidad total de la caja. Capacidad disponible: {availableCapacity}");
                    }
                }

                var transaction = _mapper.Map<BoxProductTransaction>(transactionDto);
                transaction.TransactionDate = DateTime.UtcNow;

                await _unitOfWork.BoxProductTransactions.AddAsync(transaction);

                // Actualizar última fecha de transacción del producto
                product.LastTransactionDate = transaction.TransactionDate;
                _unitOfWork.Products.Update(product);

                // Actualizar última fecha de operación de la caja
                box.LastOperationDate = transaction.TransactionDate;
                _unitOfWork.Boxes.Update(box);

                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();

                var createdTransactionDto = _mapper.Map<TransactionDTO>(transaction);
                return Result<TransactionDTO>.Success(createdTransactionDto);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Result<TransactionDTO>.Failure($"Error al crear la transacción: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<TransactionDTO>>> GetTransactionsByBoxAsync(int boxId)
        {
            try
            {
                if (!await _unitOfWork.Boxes.ExistsAsync(boxId))
                    return Result<IEnumerable<TransactionDTO>>.Failure($"No existe la caja con ID: {boxId}");

                var transactions = await _unitOfWork.BoxProductTransactions.GetTransactionsByBoxAsync(boxId);
                var transactionDtos = _mapper.Map<IEnumerable<TransactionDTO>>(transactions);
                return Result<IEnumerable<TransactionDTO>>.Success(transactionDtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<TransactionDTO>>.Failure($"Error al obtener las transacciones de la caja: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<TransactionDTO>>> GetTransactionsByProductAsync(int productId)
        {
            try
            {
                if (!await _unitOfWork.Products.ExistsAsync(productId))
                    return Result<IEnumerable<TransactionDTO>>.Failure($"No existe el producto con ID: {productId}");

                var transactions = await _unitOfWork.BoxProductTransactions.GetTransactionsByProductAsync(productId);
                var transactionDtos = _mapper.Map<IEnumerable<TransactionDTO>>(transactions);
                return Result<IEnumerable<TransactionDTO>>.Success(transactionDtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<TransactionDTO>>.Failure($"Error al obtener las transacciones del producto: {ex.Message}");
            }
        }
    }
}
