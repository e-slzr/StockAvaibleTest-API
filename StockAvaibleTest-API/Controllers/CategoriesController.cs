using Microsoft.AspNetCore.Mvc;
using StockAvaibleTest_API.DTOs;
using StockAvaibleTest_API.Services;

namespace StockAvaibleTest_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Obtiene todas las categorías
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CategoryDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCategories()
        {
            var result = await _categoryService.GetAllCategoriesAsync();
            if (!result.IsSuccess)
                return StatusCode(500, result.Error);

            return Ok(result.Data);
        }

        /// <summary>
        /// Obtiene una categoría por su ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CategoryDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCategory(int id)
        {
            var result = await _categoryService.GetCategoryByIdAsync(id);
            if (!result.IsSuccess)
                return NotFound(result.Error);

            return Ok(result.Data);
        }

        /// <summary>
        /// Crea una nueva categoría
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(CategoryDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCategory(CreateCategoryDTO categoryDto)
        {
            var result = await _categoryService.CreateCategoryAsync(categoryDto);
            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return CreatedAtAction(nameof(GetCategory), new { id = result.Data!.Id }, result.Data);
        }

        /// <summary>
        /// Actualiza una categoría existente
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(CategoryDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCategory(int id, UpdateCategoryDTO categoryDto)
        {
            var result = await _categoryService.UpdateCategoryAsync(id, categoryDto);
            if (!result.IsSuccess)
                return NotFound(result.Error);

            return Ok(result.Data);
        }

        /// <summary>
        /// Elimina una categoría
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _categoryService.DeleteCategoryAsync(id);
            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return NoContent();
        }
    }
}
