using Microsoft.AspNetCore.Mvc;
using StockAvaibleTest_API.DTOs;
using StockAvaibleTest_API.Services;

namespace StockAvaibleTest_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        /// <summary>
        /// Obtiene todas las transacciones
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TransactionDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTransactions()
        {
            var result = await _transactionService.GetAllTransactionsAsync();
            if (!result.IsSuccess)
                return StatusCode(500, result.Error);

            return Ok(result.Data);
        }

        /// <summary>
        /// Obtiene una transacción por su ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TransactionDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTransaction(int id)
        {
            var result = await _transactionService.GetTransactionByIdAsync(id);
            if (!result.IsSuccess)
                return NotFound(result.Error);

            return Ok(result.Data);
        }

        /// <summary>
        /// Crea una nueva transacción
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(TransactionDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateTransaction(CreateTransactionDTO transactionDto)
        {
            var result = await _transactionService.CreateTransactionAsync(transactionDto);
            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return CreatedAtAction(nameof(GetTransaction), new { id = result.Data!.Id }, result.Data);
        }

        /// <summary>
        /// Obtiene todas las transacciones de una caja específica
        /// </summary>
        [HttpGet("box/{boxId}")]
        [ProducesResponseType(typeof(IEnumerable<TransactionDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTransactionsByBox(int boxId)
        {
            var result = await _transactionService.GetTransactionsByBoxAsync(boxId);
            if (!result.IsSuccess)
                return NotFound(result.Error);

            return Ok(result.Data);
        }

        /// <summary>
        /// Obtiene todas las transacciones de un producto específico
        /// </summary>
        [HttpGet("product/{productId}")]
        [ProducesResponseType(typeof(IEnumerable<TransactionDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTransactionsByProduct(int productId)
        {
            var result = await _transactionService.GetTransactionsByProductAsync(productId);
            if (!result.IsSuccess)
                return NotFound(result.Error);

            return Ok(result.Data);
        }
    }
}
