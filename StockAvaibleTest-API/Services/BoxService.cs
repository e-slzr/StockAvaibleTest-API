using AutoMapper;
using StockAvaibleTest_API.Common;
using StockAvaibleTest_API.DTOs;
using StockAvaibleTest_API.Interfaces;
using StockAvaibleTest_API.Models;

namespace StockAvaibleTest_API.Services
{
    public class BoxService : IBoxService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BoxService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<BoxDTO>>> GetAllBoxesAsync()
        {
            try
            {
                var boxes = await _unitOfWork.Boxes.GetAllAsync();
                var boxDtos = _mapper.Map<IEnumerable<BoxDTO>>(boxes);
                return Result<IEnumerable<BoxDTO>>.Success(boxDtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<BoxDTO>>.Failure($"Error al obtener las cajas: {ex.Message}");
            }
        }

        public async Task<Result<BoxDetailDTO>> GetBoxByIdAsync(int id)
        {
            try
            {
                var box = await _unitOfWork.Boxes.GetBoxWithTransactionsAsync(id);
                if (box == null)
                    return Result<BoxDetailDTO>.Failure($"No se encontró la caja con ID: {id}");

                var boxDto = _mapper.Map<BoxDetailDTO>(box);

                // Obtener productos y cantidades disponibles en la caja
                var transactions = await _unitOfWork.BoxProductTransactions.GetTransactionsByBoxAsync(id);
                var productQuantities = new Dictionary<int, BoxProductQuantityDTO>();

                foreach (var transaction in transactions)
                {
                    var productId = transaction.ProductId;
                    if (!productQuantities.ContainsKey(productId))
                    {
                        var product = transaction.Product;
                        productQuantities[productId] = new BoxProductQuantityDTO
                        {
                            ProductId = productId,
                            ProductCode = product.Code,
                            ProductDescription = product.Description,
                            AvailableQuantity = 0
                        };
                    }

                    if (transaction.Type == "IN")
                        productQuantities[productId].AvailableQuantity += transaction.Quantity;
                    else
                        productQuantities[productId].AvailableQuantity -= transaction.Quantity;
                }

                boxDto.Products = productQuantities.Values.Where(p => p.AvailableQuantity > 0);
                return Result<BoxDetailDTO>.Success(boxDto);
            }
            catch (Exception ex)
            {
                return Result<BoxDetailDTO>.Failure($"Error al obtener la caja: {ex.Message}");
            }
        }

        public async Task<Result<BoxDTO>> CreateBoxAsync(CreateBoxDTO boxDto)
        {
            try
            {
                // Verificar si ya existe una caja con el mismo código
                var existingBox = await _unitOfWork.Boxes.FindAsync(b => b.Code == boxDto.Code);
                if (existingBox.Any())
                    return Result<BoxDTO>.Failure($"Ya existe una caja con el código: {boxDto.Code}");

                var box = _mapper.Map<Box>(boxDto);
                await _unitOfWork.Boxes.AddAsync(box);
                await _unitOfWork.CompleteAsync();

                var createdBoxDto = _mapper.Map<BoxDTO>(box);
                return Result<BoxDTO>.Success(createdBoxDto);
            }
            catch (Exception ex)
            {
                return Result<BoxDTO>.Failure($"Error al crear la caja: {ex.Message}");
            }
        }

        public async Task<Result<BoxDTO>> UpdateBoxAsync(int id, UpdateBoxDTO boxDto)
        {
            try
            {
                var box = await _unitOfWork.Boxes.GetByIdAsync(id);
                if (box == null)
                    return Result<BoxDTO>.Failure($"No se encontró la caja con ID: {id}");

                _mapper.Map(boxDto, box);
                _unitOfWork.Boxes.Update(box);
                await _unitOfWork.CompleteAsync();

                var updatedBoxDto = _mapper.Map<BoxDTO>(box);
                return Result<BoxDTO>.Success(updatedBoxDto);
            }
            catch (Exception ex)
            {
                return Result<BoxDTO>.Failure($"Error al actualizar la caja: {ex.Message}");
            }
        }

        public async Task<Result<bool>> DeleteBoxAsync(int id)
        {
            try
            {
                var box = await _unitOfWork.Boxes.GetByIdAsync(id);
                if (box == null)
                    return Result<bool>.Failure($"No se encontró la caja con ID: {id}");

                // Verificar si hay transacciones asociadas
                var hasTransactions = await _unitOfWork.BoxProductTransactions.FindAsync(t => t.BoxId == id);
                if (hasTransactions.Any())
                    return Result<bool>.Failure("No se puede eliminar la caja porque tiene transacciones asociadas");

                _unitOfWork.Boxes.Remove(box);
                await _unitOfWork.CompleteAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error al eliminar la caja: {ex.Message}");
            }
        }

        public async Task<Result<int>> GetProductQuantityInBoxAsync(int boxId, int productId)
        {
            try
            {
                if (!await _unitOfWork.Boxes.ExistsAsync(boxId))
                    return Result<int>.Failure($"No se encontró la caja con ID: {boxId}");

                if (!await _unitOfWork.Products.ExistsAsync(productId))
                    return Result<int>.Failure($"No se encontró el producto con ID: {productId}");

                var quantity = await _unitOfWork.Boxes.GetAvailableProductQuantityAsync(boxId, productId);
                return Result<int>.Success(quantity);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Error al obtener la cantidad del producto en la caja: {ex.Message}");
            }
        }
    }
}
