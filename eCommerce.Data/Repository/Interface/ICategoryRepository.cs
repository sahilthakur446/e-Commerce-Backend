using eCommerce.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Data.Repository.Interface
{
    public interface ICategoryRepository
    {
        Task<List<CategorySummaryDTO>> GetCategorySummaryListAsync();
        Task<List<CategoryDTO>> GetCategoryDetailsListAsync();
        Task<List<ProductDTO>> GetCategoryProductsListAsync(int categoryId);
        Task<bool> AddCategoryAsync(AddCategoryDTO categoryDTO);
        Task<bool> UpdateCategoryAsync(int id, UpdateCategoryDTO categoryDTO);
        Task<bool> DeleteCategoryAsync(int? categoryId);
    }
}
