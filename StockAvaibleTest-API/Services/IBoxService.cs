using StockAvaibleTest_API.Common;
using StockAvaibleTest_API.DTOs;

namespace StockAvaibleTest_API.Services
{
    public interface IBoxService
    {
        Task<Result<IEnumerable<BoxDTO>>> GetAllBoxesAsync();
        Task<Result<BoxDetailDTO>> GetBoxByIdAsync(int id);
        Task<Result<BoxDTO>> CreateBoxAsync(CreateBoxDTO boxDto);
        Task<Result<BoxDTO>> UpdateBoxAsync(int id, UpdateBoxDTO boxDto);
        Task<Result<bool>> DeleteBoxAsync(int id);
        Task<Result<int>> GetProductQuantityInBoxAsync(int boxId, int productId);
    }
}
