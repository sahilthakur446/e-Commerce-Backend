using eCommerce.Data.DTOs;

namespace eCommerce.Data.Repository.Interface
{
    public interface ICategoryRepository
    {
        Task<List<CategorySummaryDTO>> GetCategorySummaryListAsync();
        Task<List<CategoryWithProductCount>> GetCategoriesWithProductCountsAsync();
        Task<List<CategoryDTO>> GetCategoryDetailsListAsync();
        Task<List<ProductShowcaseDTO>> GetProductsForCategoryAsync(int categoryId);
        Task<bool> CreateCategoryAsync(AddCategoryDTO categoryDTO);
        Task<bool> UpdateCategoryAsync(int id, UpdateCategoryDTO categoryDTO);
        Task<bool> DeleteCategoryAsync(int? categoryId);
    }
}
