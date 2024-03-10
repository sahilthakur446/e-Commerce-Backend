using eCommerce.Data.Data;
using eCommerce.Data.DTOs;
using eCommerce.Data.Models;
using static eCommerce.Data.Models.Category;
using eCommerce.Data.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                        TargetGender = (int)category.CategoryTargetGender,
                        productDTOs = MapProductsToProductDTOs(category.products)
                    };
                    categoryDetails.Add(categoryDetail);
                }
                return categoryDetails;
            }
            return null;
        }

        public async Task<bool> AddCategoryAsync(AddCategoryDTO categoryDTO)
        {
            if (categoryDTO is null)
            {
                return false;
            }
            var category = new Category
            {
                CategoryName = categoryDTO.CategoryName,
                CategoryTargetGender = (TargetGender)categoryDTO.TargetGender
            };
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateCategoryAsync(AddCategoryDTO categoryDTO)
        {
           var category = await _dbContext.Categories.FindAsync(categoryDTO.CategoryId);
            if (category is null)
            {
                return false;
            }
            _dbContext.Entry(category).CurrentValues.SetValues(categoryDTO);
            category.CategoryTargetGender = (TargetGender)categoryDTO.TargetGender;
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
        private List<ProductDTO> MapProductsToProductDTOs(List<Product> products)
        {
            var productDTOs = new List<ProductDTO>();

            if (!(products is null))
            {
                foreach (var product in products)
                {
                    var productDto = new ProductDTO
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
