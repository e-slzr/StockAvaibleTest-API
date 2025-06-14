using AutoMapper;
using StockAvaibleTest_API.Common;
using StockAvaibleTest_API.DTOs;
using StockAvaibleTest_API.Interfaces;
using StockAvaibleTest_API.Models;

namespace StockAvaibleTest_API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<CategoryDTO>>> GetAllCategoriesAsync()
        {
            try
            {
                var categories = await _unitOfWork.Categories.GetAllAsync();
                var categoryDtos = _mapper.Map<IEnumerable<CategoryDTO>>(categories);
                return Result<IEnumerable<CategoryDTO>>.Success(categoryDtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<CategoryDTO>>.Failure($"Error al obtener las categorías: {ex.Message}");
            }
        }

        public async Task<Result<CategoryDTO>> GetCategoryByIdAsync(int id)
        {
            try
            {
                var category = await _unitOfWork.Categories.GetByIdAsync(id);
                if (category == null)
                    return Result<CategoryDTO>.Failure($"No se encontró la categoría con ID: {id}");

                var categoryDto = _mapper.Map<CategoryDTO>(category);
                return Result<CategoryDTO>.Success(categoryDto);
            }
            catch (Exception ex)
            {
                return Result<CategoryDTO>.Failure($"Error al obtener la categoría: {ex.Message}");
            }
        }

        public async Task<Result<CategoryDTO>> CreateCategoryAsync(CreateCategoryDTO categoryDto)
        {
            try
            {
                var category = _mapper.Map<Category>(categoryDto);

                // Verificar si ya existe una categoría con el mismo código
                var existingCategory = await _unitOfWork.Categories.FindAsync(c => c.Code == categoryDto.Code);
                if (existingCategory.Any())
                    return Result<CategoryDTO>.Failure($"Ya existe una categoría con el código: {categoryDto.Code}");

                await _unitOfWork.Categories.AddAsync(category);
                await _unitOfWork.CompleteAsync();

                var createdCategoryDto = _mapper.Map<CategoryDTO>(category);
                return Result<CategoryDTO>.Success(createdCategoryDto);
            }
            catch (Exception ex)
            {
                return Result<CategoryDTO>.Failure($"Error al crear la categoría: {ex.Message}");
            }
        }

        public async Task<Result<CategoryDTO>> UpdateCategoryAsync(int id, UpdateCategoryDTO categoryDto)
        {
            try
            {
                var category = await _unitOfWork.Categories.GetByIdAsync(id);
                if (category == null)
                    return Result<CategoryDTO>.Failure($"No se encontró la categoría con ID: {id}");

                _mapper.Map(categoryDto, category);
                _unitOfWork.Categories.Update(category);
                await _unitOfWork.CompleteAsync();

                var updatedCategoryDto = _mapper.Map<CategoryDTO>(category);
                return Result<CategoryDTO>.Success(updatedCategoryDto);
            }
            catch (Exception ex)
            {
                return Result<CategoryDTO>.Failure($"Error al actualizar la categoría: {ex.Message}");
            }
        }

        public async Task<Result<bool>> DeleteCategoryAsync(int id)
        {
            try
            {
                var category = await _unitOfWork.Categories.GetByIdAsync(id);
                if (category == null)
                    return Result<bool>.Failure($"No se encontró la categoría con ID: {id}");

                // Verificar si hay productos asociados a esta categoría
                var hasProducts = await _unitOfWork.Products.FindAsync(p => p.CategoryId == id);
                if (hasProducts.Any())
                    return Result<bool>.Failure("No se puede eliminar la categoría porque tiene productos asociados");

                _unitOfWork.Categories.Remove(category);
                await _unitOfWork.CompleteAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error al eliminar la categoría: {ex.Message}");
            }
        }
    }
}
