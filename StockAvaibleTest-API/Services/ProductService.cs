using AutoMapper;
using StockAvaibleTest_API.Common;
using StockAvaibleTest_API.DTOs;
using StockAvaibleTest_API.Interfaces;
using StockAvaibleTest_API.Models;

namespace StockAvaibleTest_API.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<ProductDTO>>> GetAllProductsAsync()
        {
            try
            {
                var products = await _unitOfWork.Products.GetProductsWithCategoryAsync();
                var productDtos = _mapper.Map<IEnumerable<ProductDTO>>(products);

                // Obtener stock disponible para cada producto
                foreach (var productDto in productDtos)
                {
                    productDto.AvailableStock = await _unitOfWork.Products.GetAvailableStockAsync(productDto.Id);
                }

                return Result<IEnumerable<ProductDTO>>.Success(productDtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<ProductDTO>>.Failure($"Error al obtener los productos: {ex.Message}");
            }
        }

        public async Task<Result<ProductDTO>> GetProductByIdAsync(int id)
        {
            try
            {
                var product = await _unitOfWork.Products.GetProductWithCategoryAsync(id);
                if (product == null)
                    return Result<ProductDTO>.Failure($"No se encontró el producto con ID: {id}");

                var productDto = _mapper.Map<ProductDTO>(product);
                productDto.AvailableStock = await _unitOfWork.Products.GetAvailableStockAsync(id);

                return Result<ProductDTO>.Success(productDto);
            }
            catch (Exception ex)
            {
                return Result<ProductDTO>.Failure($"Error al obtener el producto: {ex.Message}");
            }
        }

        public async Task<Result<ProductDTO>> CreateProductAsync(CreateProductDTO productDto)
        {
            try
            {
                // Verificar si la categoría existe
                if (!await _unitOfWork.Categories.ExistsAsync(productDto.CategoryId))
                    return Result<ProductDTO>.Failure($"No existe la categoría con ID: {productDto.CategoryId}");

                // Verificar si ya existe un producto con el mismo código
                var existingProduct = await _unitOfWork.Products.FindAsync(p => p.Code == productDto.Code);
                if (existingProduct.Any())
                    return Result<ProductDTO>.Failure($"Ya existe un producto con el código: {productDto.Code}");

                var product = _mapper.Map<Product>(productDto);
                await _unitOfWork.Products.AddAsync(product);
                await _unitOfWork.CompleteAsync();

                var createdProduct = await _unitOfWork.Products.GetProductWithCategoryAsync(product.Id);
                var createdProductDto = _mapper.Map<ProductDTO>(createdProduct);
                createdProductDto.AvailableStock = 0; // Nuevo producto, sin stock inicial

                return Result<ProductDTO>.Success(createdProductDto);
            }
            catch (Exception ex)
            {
                return Result<ProductDTO>.Failure($"Error al crear el producto: {ex.Message}");
            }
        }

        public async Task<Result<ProductDTO>> UpdateProductAsync(int id, UpdateProductDTO productDto)
        {
            try
            {
                var product = await _unitOfWork.Products.GetByIdAsync(id);
                if (product == null)
                    return Result<ProductDTO>.Failure($"No se encontró el producto con ID: {id}");

                if (!await _unitOfWork.Categories.ExistsAsync(productDto.CategoryId))
                    return Result<ProductDTO>.Failure($"No existe la categoría con ID: {productDto.CategoryId}");

                _mapper.Map(productDto, product);
                _unitOfWork.Products.Update(product);
                await _unitOfWork.CompleteAsync();

                var updatedProduct = await _unitOfWork.Products.GetProductWithCategoryAsync(id);
                var updatedProductDto = _mapper.Map<ProductDTO>(updatedProduct);
                updatedProductDto.AvailableStock = await _unitOfWork.Products.GetAvailableStockAsync(id);

                return Result<ProductDTO>.Success(updatedProductDto);
            }
            catch (Exception ex)
            {
                return Result<ProductDTO>.Failure($"Error al actualizar el producto: {ex.Message}");
            }
        }

        public async Task<Result<bool>> DeleteProductAsync(int id)
        {
            try
            {
                var product = await _unitOfWork.Products.GetByIdAsync(id);
                if (product == null)
                    return Result<bool>.Failure($"No se encontró el producto con ID: {id}");

                // Verificar si hay transacciones asociadas
                var hasTransactions = await _unitOfWork.BoxProductTransactions.FindAsync(t => t.ProductId == id);
                if (hasTransactions.Any())
                    return Result<bool>.Failure("No se puede eliminar el producto porque tiene transacciones asociadas");

                _unitOfWork.Products.Remove(product);
                await _unitOfWork.CompleteAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error al eliminar el producto: {ex.Message}");
            }
        }

        public async Task<Result<int>> GetAvailableStockAsync(int productId)
        {
            try
            {
                if (!await _unitOfWork.Products.ExistsAsync(productId))
                    return Result<int>.Failure($"No se encontró el producto con ID: {productId}");

                var stock = await _unitOfWork.Products.GetAvailableStockAsync(productId);
                return Result<int>.Success(stock);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Error al obtener el stock disponible: {ex.Message}");
            }
        }

        public async Task<Result<bool>> HasSufficientStockAsync(int productId, int quantity)
        {
            try
            {
                if (!await _unitOfWork.Products.ExistsAsync(productId))
                    return Result<bool>.Failure($"No se encontró el producto con ID: {productId}");

                var stock = await _unitOfWork.Products.GetAvailableStockAsync(productId);
                return Result<bool>.Success(stock >= quantity);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error al verificar el stock disponible: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<ProductDTO>>> GetLowStockProductsAsync()
        {
            try
            {
                var products = await _unitOfWork.Products.GetProductsWithCategoryAsync();
                var productDtos = new List<ProductDTO>();

                foreach (var product in products)
                {
                    var availableStock = await _unitOfWork.Products.GetAvailableStockAsync(product.Id);
                    if (availableStock <= product.MinimumStock)
                    {
                        var productDto = _mapper.Map<ProductDTO>(product);
                        productDto.AvailableStock = availableStock;
                        productDtos.Add(productDto);
                    }
                }

                return Result<IEnumerable<ProductDTO>>.Success(productDtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<ProductDTO>>.Failure($"Error al obtener los productos con bajo stock: {ex.Message}");
            }
        }

        public async Task<Result<ProductBoxLocationDTO>> GetProductLocationsAsync(int productId)
        {
            try
            {
                var product = await _unitOfWork.Products.GetByIdAsync(productId);
                if (product == null)
                    return Result<ProductBoxLocationDTO>.Failure($"No se encontró el producto con ID: {productId}");                var transactions = await _unitOfWork.BoxProductTransactions.GetTransactionsByProductAsync(productId);

                var boxesWithStock = transactions
                    .GroupBy(t => new { t.BoxId, t.Box.Code, t.Box.Location })
                    .Select(g => new BoxStockDTO
                    {
                        BoxId = g.Key.BoxId,
                        BoxCode = g.Key.Code,
                        Location = g.Key.Location,
                        CurrentStock = g.Sum(t => t.Type == "IN" ? t.Quantity : -t.Quantity),
                        LastTransactionDate = g.Max(t => t.TransactionDate)
                    })
                    .Where(b => b.CurrentStock > 0)
                    .ToList();

                var result = new ProductBoxLocationDTO
                {
                    ProductId = product.Id,
                    ProductCode = product.Code,
                    Description = product.Description,
                    Boxes = boxesWithStock
                };

                return Result<ProductBoxLocationDTO>.Success(result);
            }
            catch (Exception ex)
            {
                return Result<ProductBoxLocationDTO>.Failure($"Error al obtener las ubicaciones del producto: {ex.Message}");
            }
        }
    }
}
