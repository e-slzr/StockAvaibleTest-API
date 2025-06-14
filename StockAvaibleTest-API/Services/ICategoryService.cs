using StockAvaibleTest_API.Common;
using StockAvaibleTest_API.DTOs;

namespace StockAvaibleTest_API.Services
{
    public interface ICategoryService
    {
        Task<Result<IEnumerable<CategoryDTO>>> GetAllCategoriesAsync();
        Task<Result<CategoryDTO>> GetCategoryByIdAsync(int id);
        Task<Result<CategoryDTO>> CreateCategoryAsync(CreateCategoryDTO categoryDto);
        Task<Result<CategoryDTO>> UpdateCategoryAsync(int id, UpdateCategoryDTO categoryDto);
        Task<Result<bool>> DeleteCategoryAsync(int id);
    }
}
