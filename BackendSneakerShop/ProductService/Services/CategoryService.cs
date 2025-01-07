using ProductService.Abstractions;
using ProductService.Contracts;
using ProductService.Data.Entities;

namespace ProductService.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            });
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            return category is null ? null : new CategoryDto { Id = category.Id, Name = category.Name };
        }

        public async Task<CategoryDto> AddCategoryAsync(CreateCategoryDto categoryDto)
        {
            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = categoryDto.Name
            };
            await _categoryRepository.AddAsync(category);
            return new CategoryDto { Id = category.Id, Name = category.Name };
        }

        public async Task UpdateCategoryAsync(UpdateCategoryDto categoryDto)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryDto.Id);
            if (category is null) throw new KeyNotFoundException("Category not found.");

            category.Name = categoryDto.Name;
            await _categoryRepository.UpdateAsync(category);
        }

        public async Task DeleteCategoryAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category is null) throw new KeyNotFoundException("Category not found.");

            await _categoryRepository.DeleteAsync(id);
        }
    }
}
