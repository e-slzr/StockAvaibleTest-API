using Microsoft.AspNetCore.Mvc;
using StockAvaibleTest_API.DTOs;
using StockAvaibleTest_API.Services;

namespace StockAvaibleTest_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Obtiene todos los productos
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProducts()
        {
            var result = await _productService.GetAllProductsAsync();
            if (!result.IsSuccess)
                return StatusCode(500, result.Error);

            return Ok(result.Data);
        }

        /// <summary>
        /// Obtiene un producto por su ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProduct(int id)
        {
            var result = await _productService.GetProductByIdAsync(id);
            if (!result.IsSuccess)
                return NotFound(result.Error);

            return Ok(result.Data);
        }

        /// <summary>
        /// Crea un nuevo producto
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateProduct(CreateProductDTO productDto)
        {
            var result = await _productService.CreateProductAsync(productDto);
            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return CreatedAtAction(nameof(GetProduct), new { id = result.Data!.Id }, result.Data);
        }

        /// <summary>
        /// Actualiza un producto existente
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateProduct(int id, UpdateProductDTO productDto)
        {
            var result = await _productService.UpdateProductAsync(id, productDto);
            if (!result.IsSuccess)
                return NotFound(result.Error);

            return Ok(result.Data);
        }

        /// <summary>
        /// Elimina un producto
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return NoContent();
        }

        /// <summary>
        /// Obtiene el stock disponible de un producto
        /// </summary>
        [HttpGet("{id}/stock")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProductStock(int id)
        {
            var result = await _productService.GetAvailableStockAsync(id);
            if (!result.IsSuccess)
                return NotFound(result.Error);

            return Ok(result.Data);
        }

        /// <summary>
        /// Obtiene los productos con stock bajo
        /// </summary>
        [HttpGet("low-stock")]
        [ProducesResponseType(typeof(IEnumerable<ProductDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLowStockProducts()
        {
            var result = await _productService.GetLowStockProductsAsync();
            if (!result.IsSuccess)
                return StatusCode(500, result.Error);

            return Ok(result.Data);
        }
    }
}
