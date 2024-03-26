using eCommerce.Data.Data;
using eCommerce.Data.DTOs;
using eCommerce.Data.Models;
using eCommerce.Data.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eCommerce.Utilities.Enums;
using static eCommerce.Utilities.GenderConverter;


namespace eCommerce.Data.Repository.Implementation
{
    public class CategoryRepository : ICategoryRepository
        {
        private readonly ApplicationDbContext _dbContext;

        public CategoryRepository(ApplicationDbContext dbContext)
            {
            _dbContext = dbContext;
            }

        public async Task<List<CategorySummaryDTO>> GetCategorySummaryListAsync()
            {
            var categories = await _dbContext.Categories.ToListAsync();
            if (categories != null)
                {
                var categorySummaries = categories
                    .Select(c => new CategorySummaryDTO { CategoryId = c.CategoryId, CategoryName = c.CategoryName })
                    .ToList();
                if (!categorySummaries.Any())
                    {
                    throw new Exception("No categories found.");
                    }
                return categorySummaries;
                }
            return null;
            }

        public async Task<List<CategoryWithProductCount>> GetCategoriesWithProductCountsAsync()
        {
            var categories = await _dbContext.Categories.Include(c => c.products).ToListAsync();
            var categoriesWithProductCounts = new List<CategoryWithProductCount>();

            if (categories == null)
            {
                return null;
            }

            foreach (var category in categories)
            {
                var categoryDto = new CategoryWithProductCount()
                {
                    CategoryId = category.CategoryId,
                    CategoryName = category.CategoryName,
                    TargetGender = GetTargetGenderString(category.CategoryTargetGender),
                    ProductCount = category.products.Count()
                };

                categoriesWithProductCounts.Add(categoryDto);
            }

            return categoriesWithProductCounts;
        }

        public async Task<List<CategoryDTO>> GetCategoryDetailsListAsync()
            {
            var categories = await _dbContext.Categories.Include(c => c.products).ToListAsync();
            List<CategoryDTO> categoryDetails = new List<CategoryDTO>();

            if (categories.Any())
                {
                foreach (var category in categories)
                    {
                    var categoryDetail = new CategoryDTO
                        {
                        CategoryId = category.CategoryId,
                        CategoryName = category.CategoryName,
                        TargetGender = GetTargetGenderString(category.CategoryTargetGender),
                        productDTOs = MapProductsToProductDTOs(category.products)
                        };
                    categoryDetails.Add(categoryDetail);
                    }
                return categoryDetails;
                }
            return null;
            }

        public async Task<List<ProductShowcaseDTO>> GetProductsForCategoryAsync(int categoryId)
            {
            var productList = new List<ProductShowcaseDTO>();
            var category = await _dbContext.Categories.Include(category => category.products).FirstOrDefaultAsync(c => c.CategoryId == categoryId);
            if (category is null)
            {
                return null;
            }
            productList = MapProductsToProductDTOs(category.products);
            return productList;
        }
           

        public async Task<bool> CreateCategoryAsync(AddCategoryDTO categoryDTO)
        {
            if (categoryDTO is null)
            {
                return false;
            }
            var category = new Category
            {
                CategoryName = categoryDTO.CategoryName,
                CategoryTargetGender = GetTargetGender(categoryDTO.TargetGender)
            };
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateCategoryAsync(int id, UpdateCategoryDTO categoryDTO)
        {
           var category = await _dbContext.Categories.FindAsync(id);
            if (category is null)
            {
                return false;
            }
            category.CategoryName = categoryDTO.CategoryName ?? category.CategoryName;
            category.CategoryTargetGender = categoryDTO.TargetGender is not null ? GetTargetGender(categoryDTO.TargetGender): category.CategoryTargetGender;
            var changes = await _dbContext.SaveChangesAsync();
            if (changes > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteCategoryAsync(int? categoryId)
        {
            var category = await _dbContext.Categories.FindAsync(categoryId);
            if (category is null)
            {
                return false;
            }
            _dbContext.Categories.Remove(category);
            var changes = await _dbContext.SaveChangesAsync();
            if (changes > 0)
            {
                return true;
            }
            return false;
        }
        private List<ProductShowcaseDTO> MapProductsToProductDTOs(List<Product> products)
        {
            var productDTOs = new List<ProductShowcaseDTO>();

            if (!(products is null))
            {
                foreach (var product in products)
                {
                    var productDto = new ProductShowcaseDTO
                        {
                        ProductId = product.ProductId,
                        ProductName = product.ProductName,
                        ProductDescription = product.ProductDescription,
                        Price = product.Price,
                        ImagePath = product.ImagePath,
                        StockQuantity = product.StockQuantity,
                        CategoryId = product.CategoryId,
                        BrandId = product.BrandId
                    };
                    productDTOs.Add(productDto);
                }
                return productDTOs;
            }
            return null;
        }
    }
}
