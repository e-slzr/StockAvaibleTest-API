using Microsoft.AspNetCore.Mvc;
using StockAvaibleTest_API.DTOs;
using StockAvaibleTest_API.Services;

namespace StockAvaibleTest_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BoxesController : ControllerBase
    {
        private readonly IBoxService _boxService;

        public BoxesController(IBoxService boxService)
        {
            _boxService = boxService;
        }

        /// <summary>
        /// Obtiene todas las cajas
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BoxDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBoxes()
        {
            var result = await _boxService.GetAllBoxesAsync();
            if (!result.IsSuccess)
                return StatusCode(500, result.Error);

            return Ok(result.Data);
        }

        /// <summary>
        /// Obtiene una caja por su ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BoxDetailDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBox(int id)
        {
            var result = await _boxService.GetBoxByIdAsync(id);
            if (!result.IsSuccess)
                return NotFound(result.Error);

            return Ok(result.Data);
        }

        /// <summary>
        /// Crea una nueva caja
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(BoxDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateBox(CreateBoxDTO boxDto)
        {
            var result = await _boxService.CreateBoxAsync(boxDto);
            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return CreatedAtAction(nameof(GetBox), new { id = result.Data!.Id }, result.Data);
        }

        /// <summary>
        /// Actualiza una caja existente
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BoxDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateBox(int id, UpdateBoxDTO boxDto)
        {
            var result = await _boxService.UpdateBoxAsync(id, boxDto);
            if (!result.IsSuccess)
                return NotFound(result.Error);

            return Ok(result.Data);
        }

        /// <summary>
        /// Elimina una caja
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteBox(int id)
        {
            var result = await _boxService.DeleteBoxAsync(id);
            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return NoContent();
        }

        /// <summary>
        /// Obtiene la cantidad disponible de un producto en una caja específica
        /// </summary>
        [HttpGet("{boxId}/products/{productId}/quantity")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProductQuantityInBox(int boxId, int productId)
        {
            var result = await _boxService.GetProductQuantityInBoxAsync(boxId, productId);
            if (!result.IsSuccess)
                return NotFound(result.Error);

            return Ok(result.Data);
        }
    }
}
